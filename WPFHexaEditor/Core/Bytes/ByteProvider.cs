﻿//////////////////////////////////////////////
// Apache 2.0  - 2016-2019
// Author : Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using WpfHexaEditor.Core.CharacterTable;
using WpfHexaEditor.Core.MethodExtention;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Clipboard;
using DataObject = System.Windows.DataObject;
using MessageBox = System.Windows.MessageBox;
using TextDataFormat = System.Windows.TextDataFormat;

namespace WpfHexaEditor.Core.Bytes
{
    /// <summary>
    /// Used for interaction with file or stream
    /// </summary>
    public sealed class ByteProvider : IDisposable
    {
        #region Globals variable

        private readonly IDictionary<long, ByteModified> _byteModifiedDictionary = new Dictionary<long, ByteModified>();
        private string _fileName = string.Empty;
        private Stream _stream;
        private bool _readOnlyMode;
        private double _longProcessProgress;
        private string _newfilename = string.Empty;

        #endregion Globals variable

        #region Events

        public event EventHandler DataCopiedToClipboard;
        public event EventHandler ReadOnlyChanged;
        public event EventHandler Closed;
        public event EventHandler StreamOpened;
        public event EventHandler PositionChanged;
        public event EventHandler Undone;
        public event EventHandler Redone;
        public event EventHandler DataCopiedToStream;
        public event EventHandler ChangesSubmited;
        public event EventHandler LongProcessChanged;
        public event EventHandler LongProcessStarted;
        public event EventHandler LongProcessCompleted;
        public event EventHandler LongProcessCanceled;
        public event EventHandler DataPasted;
        public event EventHandler FillWithByteCompleted;
        public event EventHandler ReplaceByteCompleted;
        public event EventHandler BytesAppendCompleted;

        #endregion Events

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ByteProvider() { }

        /// <summary>
        /// Construct new ByteProvider with filename and try to open file
        /// </summary>
        public ByteProvider(string fileName) => FileName = fileName;

        /// <summary>
        /// Constuct new ByteProvider with stream
        /// </summary>
        public ByteProvider(Stream stream) => Stream = stream;

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get the type of stream are opened in byteprovider.
        /// </summary>
        public ByteProviderStreamType StreamType { get; internal set; } = ByteProviderStreamType.Nothing;

        /// <summary>
        /// Get the original length of file/stream . Return -1 if file is close.
        /// </summary>
        public long Length => IsOpen
            ? _stream.Length
            : -1;

        /// <summary>
        /// Get the length of file/stream included byte added and deleted. Return -1 if file is close.
        /// </summary>
        public long LengthAjusted => IsOpen
            ? _stream.Length - GetByteModifieds(ByteAction.Deleted).Count + GetByteModifieds(ByteAction.Added).Count
            : -1;

        /// <summary>
        /// Return true if file or stream are empty or close.
        /// </summary>
        public bool IsEmpty => !IsOpen || Length == 0;

        #region Open/close stream property/methods

        /// <summary>
        /// Set or Get the file with the control will show hex
        /// </summary>                               
        //[Obsolete("The FileName will be removed in next release.")]
        public string FileName
        {
            get => _fileName;

            set
            {
                _fileName = value;
                OpenFile();
            }
        }

        /// <summary>
        /// Get or set a MemoryStream for use with byteProvider
        /// </summary>
        public Stream Stream
        {
            get => _stream;
            set
            {
                var readonlymode = _readOnlyMode;
                Close();
                _readOnlyMode = readonlymode;

                StreamType = ByteProviderStreamType.MemoryStream;

                _stream = value;

                StreamOpened?.Invoke(this, new EventArgs());
            }
        }

        #endregion Properties

        /// <summary>
        /// Open file are set in FileName property
        /// </summary>
        public void OpenFile()
        {
            if (!File.Exists(FileName)) return;

            Close();

            try
            {
                _stream = File.Open(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }
            catch
            {
                //TODO: Localize string...
                if (MessageBox.Show("The file is locked. Do you want to open it in read-only mode?", string.Empty,
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _stream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                    ReadOnlyMode = true;
                }
            }

            StreamType = ByteProviderStreamType.File;

            StreamOpened?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Put the control on readonly mode.
        /// </summary>
        public bool ReadOnlyMode
        {
            get => _readOnlyMode;
            set
            {
                _readOnlyMode = value;

                //Launch event
                ReadOnlyChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Close stream
        /// ReadOnlyMode is reset to false
        /// </summary>
        public void Close()
        {
            if (!IsOpen) return;

            _stream.Close();
            _stream = null;
            _newfilename = string.Empty;
            ReadOnlyMode = false;
            IsOnLongProcess = false;
            LongProcessProgress = 0;

            ClearUndoChange();
            StreamType = ByteProviderStreamType.Nothing;

            Closed?.Invoke(this, new EventArgs());
        }

        #endregion Open/close stream property/methods

        #region Stream position

        /// <summary>
        /// Check if position as at EOF.
        /// </summary>
        public bool Eof => _stream.Position == _stream.Length || _stream.Position > _stream.Length;

        /// <summary>
        /// Get or Set position in file. Return -1 when file is closed
        /// </summary>
        public long Position
        {
            get => IsOpen
                ? (_stream.Position <= _stream.Length
                    ? _stream.Position
                    : _stream.Length)
                : -1;
            set
            {
                if (!IsOpen) return;

                _stream.Position = value;

                PositionChanged?.Invoke(this, new EventArgs());
            }
        }

        #endregion Stream position

        #region IsOpen property/methods

        /// <summary>
        /// Get if file is open
        /// </summary>
        public bool IsOpen => _stream != null;

        /// <summary>
        /// Get if the provider is open
        /// </summary>
        public static bool CheckIsOpen(ByteProvider provider) => provider?.IsOpen == true;

        #endregion isOpen property/methods

        #region Read byte

        /// <summary>
        /// Readbyte at position if file CanRead. Return -1 is file is closed of EOF.
        /// </summary>
        public int ReadByte() => IsOpen && _stream?.CanRead == true ? _stream.ReadByte() : -1;

        /// <summary>
        /// Read bytes, length of reading are definid with parameter count. Start at position if file CanRead. Return null is file is closed or can be read.
        /// </summary>
        public byte[] Read(int count)
        {
            if (!IsOpen) return null;
            if (!_stream.CanRead) return null;

            var countAdjusted = count;

            if (Length - Position <= count)
                countAdjusted = (int)(Length - Position);

            var bytesReaded = new byte[countAdjusted];
            _stream.Read(bytesReaded, 0, countAdjusted);

            return bytesReaded;
        }

        /// <summary>
        /// Read bytes
        /// </summary>
        public int Read(byte[] destination, int offset, int count) =>
            IsOpen && _stream.CanRead
                ? _stream.Read(destination, offset, count)
                : -1;

        #endregion read byte

        #region SubmitChanges to file/stream

        /// <summary>
        /// Submit change in a new file (Save as...)
        /// </summary>
        public bool SubmitChanges(string newFileName, bool overwrite = false)
        {
            _newfilename = string.Empty;

            if (File.Exists(newFileName) && !overwrite) return false;

            //Save as
            _newfilename = newFileName;
            File.Create(_newfilename).Close();
            SubmitChanges();
            return true;
        }


        /// <summary>
        /// Submit change to files/stream
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Qualité du code", "IDE0068:Utilisez le modèle de suppression recommandé", Justification = "<En attente>")]
        public void SubmitChanges()
        {
            if (!CanWrite)
                throw new InvalidOperationException($"Cannot write to stream while {nameof(CanWrite)} is set to false.");

            if (!Stream.CanWrite)
                throw new InvalidOperationException($"Cannot write to stream while {nameof(Stream)}.{nameof(CanWrite)} is set to false.");

            var cancel = false;

            //Launch event at process started
            IsOnLongProcess = true;
            LongProcessStarted?.Invoke(this, new EventArgs());

            //Set percent of progress to zero and create and iterator for help mesure progress
            LongProcessProgress = 0;
            int i;

            //Create appropriate temp stream for new file.
            using (var memoryStream = new MemoryStream())
            {
                var newStream = Length < ConstantReadOnly.Largefilelength
                        ? (Stream)memoryStream
                        : File.Open(Path.GetTempFileName(), FileMode.Open, FileAccess.ReadWrite);

                //Fast change only nothing byte deleted or added
                if (!GetByteModifieds(ByteAction.Deleted).Any() &&
                    !GetByteModifieds(ByteAction.Added).Any() &&
                    !File.Exists(_newfilename))
                {
                    var bytemodifiedList = GetByteModifieds(ByteAction.Modified);
                    double countChange = bytemodifiedList.Count;
                    i = 0;

                    #region Fast save. only save byteaction=modified

                    foreach (var bm in bytemodifiedList)
                        if (bm.Value.IsValid)
                        {
                            //Set percent of progress
                            LongProcessProgress = i++ / countChange;

                            //Break process?
                            if (!IsOnLongProcess)
                            {
                                cancel = true;
                                break;
                            }

                            _stream.Position = bm.Key;
                            _stream.WriteByte(bm.Value.Byte.Value);
                        }

                    #endregion
                }
                else
                {
                    //assur that we have at less 1 byte modified... if not add the first byte of file
                    if (_byteModifiedDictionary.Count == 0)
                        AddByteModified(GetByte(0).singleByte, 0);

                    byte[] buffer;
                    long bufferlength;
                    var sortedBm = GetByteModifieds(ByteAction.All).OrderBy(b => b.Key).ToList();
                    double countChange = sortedBm.Count;
                    i = 0;

                    //Set position
                    Position = 0;

                    //Start update and rewrite file.
                    foreach (var nextByteModified in sortedBm)
                    {
                        //Set percent of progress
                        LongProcessProgress = i++ / countChange;

                        //Break process?
                        if (!IsOnLongProcess) break;

                        //Reset buffer
                        buffer = new byte[ConstantReadOnly.Copyblocksize];

                        #region start read/write / use little block for optimize memory

                        while (Position != nextByteModified.Key)
                        {
                            bufferlength = nextByteModified.Key - Position;

                            //TEMPS
                            if (bufferlength < 0)
                                bufferlength = 1;

                            //EOF
                            if (bufferlength < ConstantReadOnly.Copyblocksize)
                                buffer = new byte[bufferlength];

                            _stream.Read(buffer, 0, buffer.Length);
                            newStream.Write(buffer, 0, buffer.Length);
                        }

                        #endregion

                        #region Apply ByteAction!

                        switch (nextByteModified.Value.Action)
                        {
                            case ByteAction.Added:
                                //TODO : IMPLEMENTING ADD BYTE
                                break;
                            case ByteAction.Deleted:
                                Position++;
                                break;
                            case ByteAction.Modified:
                                Position++;
                                newStream.WriteByte(nextByteModified.Value.Byte.Value);
                                break;
                        }

                        #endregion

                        #region Read/Write the last section of file

                        if (nextByteModified.Key == sortedBm.Last().Key)
                        {
                            while (!Eof)
                            {
                                bufferlength = _stream.Length - Position;

                                //EOF
                                if (bufferlength < ConstantReadOnly.Copyblocksize)
                                    buffer = new byte[bufferlength];

                                _stream.Read(buffer, 0, buffer.Length);
                                newStream.Write(buffer, 0, buffer.Length);
                            }
                        }

                        #endregion
                    }

                    #region Set stream to new file (save as)

                    var refreshByteProvider = false;
                    if (File.Exists(_newfilename))
                    {
                        _stream?.Close();
                        _stream = File.Open(_newfilename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                        _stream?.SetLength(newStream.Length);
                        refreshByteProvider = true;
                    }

                    #endregion

                    #region Write new data to current stream

                    Position = 0;
                    newStream.Position = 0;
                    buffer = new byte[ConstantReadOnly.Copyblocksize];

                    while (!Eof)
                    {
                        //Set percent of progress
                        LongProcessProgress = Position / (double)Length;

                        //Break process?
                        if (!IsOnLongProcess)
                        {
                            cancel = true;
                            break;
                        }

                        bufferlength = _stream.Length - Position;

                        //EOF
                        if (bufferlength < ConstantReadOnly.Copyblocksize)
                            buffer = new byte[bufferlength];

                        newStream.Read(buffer, 0, buffer.Length);
                        _stream.Write(buffer, 0, buffer.Length);
                    }
                    _stream.SetLength(newStream.Length);

                    #endregion

                    //dispose resource
                    newStream.Close();

                    if (refreshByteProvider)
                        FileName = _newfilename;
                }
            }

            //Launch event at process completed
            if (cancel)
                LongProcessCanceled?.Invoke(this, new EventArgs());
            else
                LongProcessCompleted?.Invoke(this, new EventArgs());

            //Launch event
            ChangesSubmited?.Invoke(this, new EventArgs());
        }

        #endregion SubmitChanges to file/stream

        #region Bytes modifications methods

        /// <summary>
        /// Check if the byte in parameter are modified and return original Bytemodified from list
        /// </summary>
        public (bool success, ByteModified val) CheckIfIsByteModified(long bytePositionInStream,
                                                                      ByteAction action = ByteAction.Modified) =>
            _byteModifiedDictionary.TryGetValue(bytePositionInStream, out var byteModified)
                    && byteModified.IsValid
                    && (byteModified.Action == action || action == ByteAction.All)
                        ? (true, byteModified)
                        : (false, null);

        /// <summary>
        /// Add/Modifiy a ByteModifed in the list of byte have changed
        /// </summary>
        public void AddByteModified(byte? @byte, long BytePositionInStream, long undoLength = 1)
        {
            var (success, _) = CheckIfIsByteModified(BytePositionInStream);

            if (success)
                _byteModifiedDictionary.Remove(BytePositionInStream);

            var byteModified = new ByteModified
            {
                Byte = @byte,
                Length = undoLength,
                BytePositionInStream = BytePositionInStream,
                Action = ByteAction.Modified
            };

            try
            {
                _byteModifiedDictionary.Add(BytePositionInStream, byteModified);
                UndoStack.Push(byteModified);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Add/Modifiy a ByteModifed in the list of byte have deleted
        /// </summary>
        /// <returns>
        /// Return the last position
        /// </returns>
        public long AddByteDeleted(long bytePositionInStream, long length)
        {
            var position = bytePositionInStream;

            for (var i = 0; i < length; i++)
            {
                if (i % 100 == 0) Application.Current.DoEvents();

                var (success, _) = CheckIfIsByteModified(position, ByteAction.All);

                if (success)
                    _byteModifiedDictionary.Remove(position);

                var byteModified = new ByteModified
                {
                    Byte = new byte(),
                    Length = length,
                    BytePositionInStream = position,
                    Action = ByteAction.Deleted
                };
                _byteModifiedDictionary.Add(position, byteModified);

                UndoStack.Push(byteModified);

                position++;
            }

            return position;
        }


        /// <summary>
        /// Return an IEnumerable ByteModified have action set to Modified
        /// </summary>
        public IDictionary<long, ByteModified> GetByteModifieds(ByteAction action) =>
            action == ByteAction.All
                ? _byteModifiedDictionary
                : _byteModifiedDictionary.Where(b => b.Value.Action == action).ToDictionary(k => k.Key, v => v.Value);

        /// <summary>
        /// Fill with byte at position
        /// </summary>
        /// <param name="startPosition">The position to start fill</param>
        /// <param name="length">The length to fill</param>
        /// <param name="b">the byte to fill</param>
        public void FillWithByte(long startPosition, long length, byte b)
        {
            //Launch event at process strated
            IsOnLongProcess = true;
            LongProcessStarted?.Invoke(this, new EventArgs());

            Position = startPosition;

            if (Position > -1)
            {
                for (var i = 0; i < length; i++)
                {
                    if (Eof) break;

                    //Do not freeze UI...
                    if (i % 2000 == 0)
                        LongProcessProgress = (double)i / length;

                    //Break long process if needed
                    if (!IsOnLongProcess)
                        break;

                    Position = startPosition + i;
                    if (GetByte(Position).singleByte != b)
                        AddByteModified(b, Position - 1);

                }

                FillWithByteCompleted?.Invoke(this, new EventArgs());
            }

            //Launch event at process completed
            IsOnLongProcess = false;
            LongProcessCompleted?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Replace byte with another in selection
        /// </summary>
        /// <param name="startPosition">The position to start fill</param>
        /// <param name="length">The length of the selection</param>
        public void ReplaceByte(long startPosition, long length, byte original, byte replace)
        {
            //Launch event at process strated
            IsOnLongProcess = true;
            LongProcessStarted?.Invoke(this, new EventArgs());

            Position = startPosition;

            if (Position > -1)
            {
                for (var i = 0; i < length; i++)
                {
                    if (Eof) break;

                    //Do not freeze UI...
                    if (i % 2000 == 0)
                        LongProcessProgress = (double)i / length;

                    //Break long process if needed
                    if (!IsOnLongProcess)
                        break;

                    Position = startPosition + i;
                    if (GetByte(Position).singleByte == original)
                        AddByteModified(replace, Position - 1);
                }

                ReplaceByteCompleted?.Invoke(this, new EventArgs());
            }

            //Launch event at process completed
            IsOnLongProcess = false;
            LongProcessCompleted?.Invoke(this, new EventArgs());
        }

        #endregion Bytes modifications methods

        #region Copy/Paste/Cut Methods

        /// <summary>
        /// Get the length of byte are selected (base 1)
        /// </summary>
        public static long GetSelectionLength(long selectionStart, long selectionStop)
        {
            if (selectionStart == -1 || selectionStop == -1) return 0;
            if (selectionStart == selectionStop) return 1;

            return selectionStart > selectionStop
                ? selectionStart - selectionStop + 1
                : selectionStop - selectionStart + 1;
        }

        /// <summary>
        /// Get the byte at selection start.
        /// </summary>
        /// <param name="copyChange">if true take bytemodified in operation</param>
        public (byte? singleByte, bool succes) GetByte(long position, bool copyChange = true)
        {
            if (!CanCopy(position, position)) return (null, false);
            if (position <= -1) return (null, false);

            var buffer = GetCopyData(position, position, copyChange);

            if (buffer.Any())
                return (buffer[0], true);

            return (null, false);
        }

        /// <summary>
        /// Copies the current selection in the hex box to the Clipboard.
        /// </summary>
        /// <param name="copyChange">Set tu true if you want include change in your copy. Set to false to copy directly from source</param>
        public byte[] GetCopyData(long selectionStart, long selectionStop, bool copyChange)
        {
            //Validation
            if (!CanCopy(selectionStart, selectionStop)) return Array.Empty<byte>();
            if (selectionStart == -1 || selectionStop == -1) return Array.Empty<byte>();

            //Variable
            var bufferList = new List<byte>();

            #region Set start position

            _stream.Position = selectionStart != selectionStop
                ? (selectionStart > selectionStop
                    ? selectionStop
                    : selectionStart)
                : selectionStart;

            #endregion

            //Exclude byte deleted from copy
            if (!copyChange)
            {
                var buffer = new byte[GetSelectionLength(selectionStart, selectionStop)];
                _stream.Read(buffer, 0, Convert.ToInt32(GetSelectionLength(selectionStart, selectionStop)));
                return buffer;
            }

            for (var i = 0; i < GetSelectionLength(selectionStart, selectionStop); i++)
            {
                var (success, byteModified) = CheckIfIsByteModified(_stream.Position, ByteAction.All);

                if (!success)
                {
                    bufferList.Add((byte)_stream.ReadByte());
                    continue;
                }

                switch (byteModified.Action)
                {
                    case ByteAction.Modified:
                        if (byteModified.IsValid) bufferList.Add(byteModified.Byte.Value);
                        break;
                        //case ByteAction.Deleted: //NOTHING to do we dont want to add deleted byte   
                        //case ByteAction.Added: //TODO : IMPLEMENTING ADD BYTE       
                        //    break;
                }

                _stream.Position++;
            }

            return bufferList.ToArray();
        }

        /// <summary>
        /// Copy selection of byte to clipboard
        /// </summary>
        /// <param name="copyChange">Set to true if you want onclude change in your copy. Set to false to copy directly from source</param>
        public void CopyToClipboard(CopyPasteMode copypastemode,
                                    long selectionStart,
                                    long selectionStop,
                                    bool copyChange = true,
                                    TblStream tbl = null)
        {
            if (!CanCopy(selectionStart, selectionStop)) return;

            //Variables
            var buffer = GetCopyData(selectionStart, selectionStop, copyChange);
            string sBuffer;

            var da = new DataObject();

            switch (copypastemode)
            {
                case CopyPasteMode.Byte:
                    throw new NotImplementedException();
                case CopyPasteMode.TblString when tbl != null:
                    sBuffer = tbl.ToTblString(buffer);
                    da.SetText(sBuffer, TextDataFormat.Text);
                    break;
                case CopyPasteMode.AsciiString:
                    sBuffer = ByteConverters.BytesToString(buffer);
                    da.SetText(sBuffer, TextDataFormat.Text);
                    break;
                case CopyPasteMode.HexaString:
                    sBuffer = ByteConverters.ByteToHex(buffer);
                    da.SetText(sBuffer, TextDataFormat.Text);
                    break;
                case CopyPasteMode.CSharpCode:
                    CopyToClipboard_Language(selectionStart, selectionStop, copyChange, da, CodeLanguage.CSharp);
                    break;
                case CopyPasteMode.CCode:
                    CopyToClipboard_Language(selectionStart, selectionStop, copyChange, da, CodeLanguage.C);
                    break;
                case CopyPasteMode.JavaCode:
                    CopyToClipboard_Language(selectionStart, selectionStop, copyChange, da, CodeLanguage.Java);
                    break;
                case CopyPasteMode.FSharpCode:
                    CopyToClipboard_Language(selectionStart, selectionStop, copyChange, da, CodeLanguage.FSharp);
                    break;
                case CopyPasteMode.VbNetCode:
                    CopyToClipboard_Language(selectionStart, selectionStop, copyChange, da, CodeLanguage.Vbnet);
                    break;
                case CopyPasteMode.PascalCode:
                    CopyToClipboard_Language(selectionStart, selectionStop, copyChange, da, CodeLanguage.Pascal);
                    break;
            }

            //set memorystream (BinaryData) clipboard data
            using (var ms = new MemoryStream(buffer, 0, buffer.Length, false, true))
                da.SetData("BinaryData", ms);

            Clipboard.SetDataObject(da, true);

            DataCopiedToClipboard?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Copy selection to clipboard in code block
        /// </summary>
        private void CopyToClipboard_Language(long selectionStart, long selectionStop, bool copyChange, DataObject da, CodeLanguage language)
        {
            if (!CanCopy(selectionStart, selectionStop)) return;

            //Variables
            var buffer = GetCopyData(selectionStart, selectionStop, copyChange);
            var i = 0;
            var length = GetSelectionLength(selectionStart, selectionStop);
            var delimiter = language == CodeLanguage.FSharp ? ";" : ",";

            var sb = new StringBuilder();

            #region define header

            switch (language)
            {
                case CodeLanguage.C:
                case CodeLanguage.CSharp:
                case CodeLanguage.Java:
                    sb.Append(
                        $"/* {FileName} ({DateTime.Now.ToString(CultureInfo.CurrentCulture)}), \r\n StartPosition: 0x{ByteConverters.LongToHex(selectionStart)}, StopPosition: 0x{ByteConverters.LongToHex(selectionStop)}, Length: 0x{ByteConverters.LongToHex(length)} */");
                    break;
                case CodeLanguage.Vbnet:
                    sb.Append(
                        $"' {FileName} ({DateTime.Now.ToString(CultureInfo.CurrentCulture)}), \r\n' StartPosition: &H{ByteConverters.LongToHex(selectionStart)}, StopPosition: &H{ByteConverters.LongToHex(selectionStop)}, Length: &H{ByteConverters.LongToHex(length)}");
                    break;
                case CodeLanguage.Pascal:
                    sb.Append(
                        "{ " + $" {FileName} ({DateTime.Now.ToString(CultureInfo.CurrentCulture)}), \r\n   StartPosition: 0x{ByteConverters.LongToHex(selectionStart)}, StopPosition: 0x{ByteConverters.LongToHex(selectionStop)}, Length: 0x{ByteConverters.LongToHex(length)}" + " }");
                    break;
                case CodeLanguage.FSharp:
                    sb.Append(
                        $"// {FileName} ({DateTime.Now.ToString(CultureInfo.CurrentCulture)}), \r\n// StartPosition: 0x{ByteConverters.LongToHex(selectionStart)}, StopPosition: 0x{ByteConverters.LongToHex(selectionStop)}, Length: 0x{ByteConverters.LongToHex(length)}");
                    break;
            }

            #endregion

            sb.AppendLine();
            sb.AppendLine();

            #region define string representation of copied data

            switch (language)
            {
                case CodeLanguage.CSharp:
                    sb.Append($"string sData =\"{ByteConverters.BytesToString(buffer)}\";");
                    sb.AppendLine();
                    sb.Append(
                        $"string sDataHex =\"{ByteConverters.StringToHex(ByteConverters.BytesToString(buffer))}\";");
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.Append("byte[] rawData = {");
                    sb.AppendLine();
                    sb.Append("\t");
                    break;
                case CodeLanguage.Java:
                    sb.Append($"String sData =\"{ByteConverters.BytesToString(buffer)}\";");
                    sb.AppendLine();
                    sb.Append(
                        $"String sDataHex =\"{ByteConverters.StringToHex(ByteConverters.BytesToString(buffer))}\";");
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.Append("byte rawData[] = {");
                    sb.AppendLine();
                    sb.Append("\t");
                    break;
                case CodeLanguage.C:
                    sb.Append($"char sData[] =\"{ByteConverters.BytesToString(buffer)}\";");
                    sb.AppendLine();
                    sb.Append(
                        $"char sDataHex[] =\"{ByteConverters.StringToHex(ByteConverters.BytesToString(buffer))}\";");
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.Append($"unsigned char rawData[{length}] {{");
                    sb.AppendLine();
                    sb.Append("\t");
                    break;
                case CodeLanguage.FSharp:
                    sb.Append($"let sData = @\"{ByteConverters.BytesToString(buffer)}\";");
                    sb.AppendLine();
                    sb.Append(
                        $"let sDataHex = @\"{ByteConverters.StringToHex(ByteConverters.BytesToString(buffer))}\";");
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.Append("let bytes = [|");
                    sb.AppendLine();
                    sb.Append("    ");
                    break;
                case CodeLanguage.Vbnet:
                    sb.Append($"Dim sData as String =\"{ByteConverters.BytesToString(buffer)}\";");
                    sb.AppendLine();
                    sb.Append(
                        $"Dim sDataHex as String =\"{ByteConverters.StringToHex(ByteConverters.BytesToString(buffer))}\";");
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.Append("Dim rawData As Byte() = { _");
                    sb.AppendLine();
                    sb.Append("\t");
                    break;
                case CodeLanguage.Pascal:
                    sb.Append($"sData: string = @\'{ByteConverters.BytesToString(buffer)}\';");
                    sb.AppendLine();
                    sb.Append(
                        $"sDataHex: string = @\'{ByteConverters.StringToHex(ByteConverters.BytesToString(buffer))}\';");
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.Append($"RawData: array[0..{buffer.Length - 1}] of Byte = (");
                    sb.AppendLine();
                    sb.Append("  ");
                    break;
            }

            #endregion

            #region Build data array

            foreach (var b in buffer)
            {
                i++;
                if (language == CodeLanguage.Java) sb.Append("(byte)");

                #region Append byte
                var byteStr = language switch
                {
                    CodeLanguage.Vbnet => $"&H{ByteConverters.ByteToHex(b)}, ",
                    CodeLanguage.Pascal => $"${ByteConverters.ByteToHex(b)}, ",
                    _ => $"0x{ByteConverters.ByteToHex(b)}{delimiter} ",
                };

                sb.Append(byteStr);
                #endregion

                if (i == (language == CodeLanguage.Java ? 6 : 12))
                {
                    i = 0;
                    if (language == CodeLanguage.Vbnet) sb.Append("_");
                    sb.AppendLine();
                    sb.Append(language != CodeLanguage.FSharp ? "\t" : "    ");
                }
            }
            if (language == CodeLanguage.Vbnet) sb.Append("_");
            sb.AppendLine();

            #region End of block
            var sByteEnd = language switch
            {
                CodeLanguage.FSharp => "|]",
                CodeLanguage.Pascal => ");",
                _ => "};",
            };
            sb.Append(sByteEnd);
            #endregion

            da.SetText(sb.ToString(), TextDataFormat.Text);

            #endregion
        }

        /// <summary>
        /// Copy selection of byte to a stream
        /// </summary>
        /// <param name="output">Output stream. Data will be copied at end of stream</param>
        /// <param name="copyChange">Set tu true if you want onclude change in your copy. Set to false to copy directly from source</param>
        public void CopyToStream(Stream output, long selectionStart, long selectionStop, bool copyChange = true)
        {
            if (!CanCopy(selectionStart, selectionStop)) return;
            if (output is null) return;

            //Variables
            var buffer = GetCopyData(selectionStart, selectionStop, copyChange);

            if (output.CanWrite)
                output.Write(buffer, (int)output.Length, buffer.Length);
            else
                throw new IOException(Properties.Resources.WritingErrorExeptionString);

            DataCopiedToStream?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Get all byte from byte provider...
        /// </summary>
        public byte[] GetAllBytes(bool copyChange = true) => GetCopyData(0, Length - 1, copyChange);

        /// <summary>
        /// Paste the string at position with posibility to expend and append at end of file
        /// </summary>
        /// <param name="pasteString">The string to paste</param>
        /// <param name="startPosition">The position to start pasting. Return if position is less than zero </param>
        /// <param name="expend">If true expend the file if needed, ATTENTION: bytes expended can't be canceled with undo</param>
        public void Paste(long startPosition, string pasteString, bool expend)
        {
            if (startPosition < 0) return;
            if (pasteString is null) return;

            long pastelength = pasteString.Length;
            Position = startPosition;
            var i = 0;

            //Expend if needed
            if (Position + pastelength > Length && expend)
            {
                var lengthToExpend = Position - Length + pastelength;
                AppendByte(0, lengthToExpend);
                Position = startPosition;
            }

            //Start to paste the string
            if (Position > -1)
            {
                foreach (var chr in pasteString)
                    if (!Eof)
                    {
                        Position = startPosition + i++;
                        if (GetByte(Position).singleByte != ByteConverters.CharToByte(chr))
                            AddByteModified(ByteConverters.CharToByte(chr), Position - 1, pastelength);
                    }
                    else
                        break;

                DataPasted?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Paste the byte array at position with posibility to expend and append at end of file
        /// </summary>
        /// <param name="pasteBytes">The byte array to paste</param>
        /// <param name="startPosition">The position to start pasting</param>
        /// <param name="expend">If true expend the file if needed, ATTENTION: bytes expended can't be canceled with undo</param>
        public void Paste(long startPosition, byte[] pasteBytes, bool expend)
        {
            if (pasteBytes is null) return;

            long pastelength = pasteBytes.Length;
            Position = startPosition;
            var i = 0;

            //Expend if needed
            if (Position + pastelength > Length && expend)
            {
                var lengthToExpend = Position - Length + pastelength;
                AppendByte(0, lengthToExpend);
                Position = startPosition;
            }

            //Start to paste the string
            if (Position > -1)
            {
                foreach (var bt in pasteBytes)
                    if (!Eof)
                    {
                        Position = startPosition + i++;
                        if (GetByte(Position).singleByte != bt)
                            AddByteModified(bt, Position - 1, pastelength);
                    }
                    else
                        break;

                DataPasted?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Paste the string at position
        /// </summary>
        /// <param name="pasteString">The string to paste</param>
        public void PasteNotInsert(string pasteString) => Paste(Position, pasteString, false);

        /// <summary>
        /// Paste the string at position
        /// </summary>
        /// <param name="pasteString">The string to paste</param>
        /// <param name="position">Start position</param>
        public void PasteNotInsert(long position, string pasteString) => Paste(position, pasteString, false);

        /// <summary>
        /// Paste the bytes array at position
        /// </summary>
        /// <param name="pasteBytes">The bytes array to paste</param>
        /// <param name="position">Start position</param>
        public void PasteNotInsert(long position, byte[] pasteBytes) => Paste(position, pasteBytes, false);

        /// <summary>
        /// Paste the bytes array at actual position
        /// </summary>
        /// <param name="pasteBytes">The bytes array to paste</param>
        public void PasteNotInsert(byte[] pasteBytes) => Paste(Position, pasteBytes, false);

        #endregion Copy/Paste/Cut Methods

        #region Undo / Redo

        /// <summary>
        /// Undo last byteaction
        /// </summary>
        /// <returns>Return a list of long contening the position are undone.</returns>
        public void Undo()
        {
            if (!CanUndo) return;

            var last = UndoStack.Pop();
            var bytePositionList = new List<long>();
            var undoLength = last.Action != ByteAction.Deleted ? last.Length : 1;

            bytePositionList.Add(last.BytePositionInStream);
            _byteModifiedDictionary.Remove(last.BytePositionInStream);
            RedoStack.Push(last);

            if (undoLength > 1)
                for (var i = 0; i < undoLength; i++)
                    if (UndoStack.Count > 0)
                    {
                        last = UndoStack.Pop();
                        bytePositionList.Add(last.BytePositionInStream);
                        _byteModifiedDictionary.Remove(last.BytePositionInStream);
                        RedoStack.Push(last);
                    }

            Undone?.Invoke(bytePositionList, new EventArgs());
        }

        /// <summary>
        /// Redo last action made with redo...
        /// </summary>
        /// <returns>Return a list of long contening the position are redone.</returns>
        public void Redo()
        {
            if (CanRedo)
            {
                var last = RedoStack.Pop();
                var bytePositionList = new List<long>();
                var redoLength = last.Action != ByteAction.Deleted ? last.Length : 1;

                bytePositionList.Add(last.BytePositionInStream);
                addUndo(last);

                if (redoLength > 1)
                    for (var i = 0; i < redoLength; i++)
                        if (RedoStack.Count > 0)
                        {
                            last = RedoStack.Pop();
                            bytePositionList.Add(last.BytePositionInStream);

                            addUndo(last);
                        }

                Redone?.Invoke(bytePositionList, new EventArgs());
            }

            #region local fonction
            void addUndo(ByteModified last)
            {
                //add undo...
                switch (last.Action)
                {
                    case ByteAction.Deleted:
                        AddByteDeleted(last.BytePositionInStream, last.Length);
                        break;
                    case ByteAction.Modified:
                        AddByteModified(last.Byte, last.BytePositionInStream, last.Length);
                        break;
                }
            }
            #endregion
        }

        /// <summary>
        /// Clear all changes
        /// </summary>
        public void ClearUndoChange()
        {
            _byteModifiedDictionary?.Clear();
            UndoStack?.Clear();
            ClearRedoChange();
        }

        /// <summary>
        /// Clear changes and Redo
        /// </summary>
        public void ClearRedoChange() => RedoStack?.Clear();

        /// <summary>
        /// Gets the undo count.
        /// </summary>
        public int UndoCount => UndoStack.Count;

        /// <summary>
        /// Gets the redo count.
        /// </summary>
        public int RedoCount => RedoStack.Count;

        /// <summary>
        /// Gets the undo stack.
        /// </summary>
        public Stack<ByteModified> UndoStack { get; } = new Stack<ByteModified>();

        /// <summary>
        /// Gets the redo stack.
        /// </summary>
        public Stack<ByteModified> RedoStack { get; } = new Stack<ByteModified>();

        /// <summary>
        /// Get or set for indicate if control CanUndo
        /// </summary>
        public bool IsUndoEnabled { get; set; } = true;

        /// <summary>
        /// Get or set for indicate if control CanRedo
        /// </summary>
        public bool IsRedoEnabled { get; set; } = true;

        /// <summary>
        /// Check if the control can undone to a previous value
        /// </summary>
        public bool CanUndo => IsUndoEnabled && UndoStack.Count > 0;

        /// <summary>
        /// Check if the control can redone to a previous value
        /// </summary>
        public bool CanRedo => IsRedoEnabled && RedoStack.Count > 0;

        #endregion Undo / Redo

        #region Various can do property...

        /// <summary>
        /// Return true if Copy method could be invoked.
        /// </summary>
        public bool CanCopy(long selectionStart, long selectionStop) =>
            GetSelectionLength(selectionStart, selectionStop) >= 1 && IsOpen;

        /// <summary>
        /// Update a value indicating whether the current stream is supporting writing.
        /// </summary>
        public bool CanWrite => _stream != null && !ReadOnlyMode && _stream.CanWrite;

        /// <summary>
        /// Update a value indicating  whether the current stream is supporting reading.
        /// </summary>
        public bool CanRead => _stream != null && _stream.CanRead;

        /// <summary>
        /// Update a value indicating  whether the current stream is supporting seeking.
        /// </summary>
        public bool CanSeek => _stream != null && _stream.CanSeek;

        #endregion Can do property...

        #region Find methods

        /// <summary>
        /// Find all occurance of string in stream and return an IEnumerable contening index when is find.
        /// </summary>
        public IEnumerable<long> FindIndexOf(string stringToFind, long startPosition = 0) =>
            FindIndexOf(ByteConverters.StringToByte(stringToFind), startPosition);

        /// <summary>
        /// Find all occurance of byte[] in stream and return an IEnumerable contening index when is find.
        /// </summary>
        public IEnumerable<long> FindIndexOf(byte[] bytesTofind, long startPosition = 0)
        {
            //Validation
            if (bytesTofind is null || bytesTofind.Length == 0) yield break;

            //start position checkup
            if (startPosition > Length) startPosition = Length;
            else if (startPosition < 0) startPosition = 0;

            //var
            Position = startPosition;
            var buffer = new byte[ConstantReadOnly.Findblocksize];
            var cancel = false;

            //Launch event at process strated
            IsOnLongProcess = true;
            LongProcessStarted?.Invoke(this, new EventArgs());

            //start find
            for (var i = startPosition; i < Length; i++)
            {
                //Do not freeze UI...
                if (i % 200 == 0)
                    LongProcessProgress = (double)Position / Length;

                //Break long process if needed
                if (!IsOnLongProcess)
                {
                    cancel = true;
                    break;
                }

                if ((byte)ReadByte() == bytesTofind[0])
                {
                    //position correction after read one byte
                    Position--;
                    i--;

                    //buffer length corection
                    if (buffer.Length > Length - Position)
                        buffer = new byte[Length - Position];

                    //read buffer and find
                    _stream.Read(buffer, 0, buffer.Length);
                    var findindex = buffer.FindIndexOf(bytesTofind).ToList();

                    //if byte if find add to Yield return finded occurence
                    if (findindex.Any())
                        foreach (var index in findindex)
                            yield return index + i + 1;

                    //position correction
                    i += buffer.Length;
                }
            }

            IsOnLongProcess = false;

            //Launch event at process completed
            if (cancel)
                LongProcessCanceled?.Invoke(this, new EventArgs());
            else
                LongProcessCompleted?.Invoke(this, new EventArgs());
        }

        #endregion Find methods

        #region Long process progress

        /// <summary>
        /// Get if byteprovider is on a long process. Set to false to cancel all process.
        /// </summary>
        public bool IsOnLongProcess { get; internal set; }

        /// <summary>
        /// Get the long progress percent of job.
        /// When set (internal) launch event LongProcessProgressChanged
        /// </summary>
        public double LongProcessProgress
        {
            get => _longProcessProgress;

            internal set
            {
                _longProcessProgress = value;
                LongProcessChanged?.Invoke(value, new EventArgs());
            }
        }

        #endregion Long process progress

        #region IDisposable Support

        private bool _disposedValue; // Pour détecter les appels redondants

        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
                IsOnLongProcess = false;
                _stream = null;
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support

        #region Computing count byte methods...

        /// <summary>
        /// Get an array of long computing the total of each byte in the file. 
        /// The position of the array makes it possible to obtain the sum of the desired byte
        /// </summary>
        public long[] GetByteCount()
        {
            if (!IsOpen) return null;

            //Launch event at process strated
            IsOnLongProcess = true;
            LongProcessStarted?.Invoke(this, new EventArgs());

            const int copyBufferSize = 1024 * 1024;
            var cancel = false;
            var buffer = new byte[copyBufferSize];
            var storedCnt = new long[256];
            int count;

            Position = 0;

            while ((count = Read(buffer, 0, copyBufferSize)) > 0)
            {
                for (var i = 0; i < count; i++)
                    storedCnt[buffer[i]]++;

                //Do not freeze UI...
                if (Position % 2000 == 0)
                    LongProcessProgress = (double)Position / Length;

                //Break long process if needed
                if (!IsOnLongProcess)
                {
                    cancel = true;
                    break;
                }
            }

            IsOnLongProcess = false;

            //Launch event at process completed
            if (cancel)
            {
                LongProcessCanceled?.Invoke(this, new EventArgs());
                return null;
            }

            LongProcessCompleted?.Invoke(this, new EventArgs());

            return storedCnt;
        }

        #endregion

        #region Append byte at end of file

        /// <summary>
        /// Append byte at end of file
        /// </summary>
        public void AppendByte(byte[] bytesToAppend)
        {
            if (bytesToAppend == null) return;

            _stream.Position = _stream.Length;
            _stream.SetLength(Length + bytesToAppend.Length);

            foreach (byte b in bytesToAppend)
                _stream.WriteByte(b);

            BytesAppendCompleted?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Append byte at end of file
        /// </summary>
        public void AppendByte(byte byteToAppend, long count = 1)
        {
            _stream.Position = _stream.Length;
            _stream.SetLength(Length + count);

            for (var i = 0; i < count; i++)
                _stream.WriteByte(byteToAppend);

            BytesAppendCompleted?.Invoke(this, new EventArgs());
        }

        #endregion Append byte at end of file

        #region Reverse bytes selection

        /// <summary>
        /// Reverse bytes array like this {AA, FF, EE, DC} => {DC, EE, FF, AA}
        /// </summary>
        public void Reverse(long selectionStart, long selectionStop)
        {
            var data = GetCopyData(selectionStart, selectionStop, true);

            #region Set start position

            var startPosition =
                (selectionStart != selectionStop
                    ? (selectionStart > selectionStop
                        ? selectionStop
                        : selectionStart)
                    : selectionStart
                ) + data.Length;

            #endregion

            foreach (byte b in data)
                AddByteModified(b, --startPosition, data.Length);
        }

        #endregion
    }
}