//////////////////////////////////////////////
// Apache 2.0  - 2016-2020
// Author : Derek Tremblay (derektremblay666@gmail.com)
// Contributor: Janus Tida
//////////////////////////////////////////////

using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfHexaEditor.Core;
using WpfHexaEditor.Core.Bytes;
using WpfHexaEditor.Core.MethodExtention;

namespace WpfHexaEditor
{
    internal class HexByte : BaseByte
    {
        #region Global class variables

        private KeyDownLabel _keyDownLabel = KeyDownLabel.FirstChar;

        #endregion global class variables

        #region Constructor

        public HexByte(HexEditor parent) : base(parent)
        {
            //Update width
            UpdateDataVisualWidth();
        }

        #endregion Contructor

        #region Methods

        /// <summary>
        /// Update the render of text derived bytecontrol from byte property
        /// </summary>
        public override void UpdateTextRenderFromByte()
        {
            if (Byte != null)
            {
                switch (_parent.DataStringVisual)
                {
                    case DataVisualType.Hexadecimal:
                        var chArr = ByteConverters.ByteToHexCharArray(Byte.Value);
                        Text = new string(chArr);
                        break;
                    case DataVisualType.Decimal:
                        Text = Byte.Value.ToString("d3");
                        break;
                    case DataVisualType.Binary:
                        Text = Convert.ToString(Byte.Value, 2).PadLeft(8, '0');
                        break;
                }
            }
            else
                Text = string.Empty;
        }

        public override void Clear()
        {
            base.Clear();
            _keyDownLabel = KeyDownLabel.FirstChar;
        }

        public void UpdateDataVisualWidth() => Width = _parent.DataStringVisual switch
        {
            DataVisualType.Decimal => 25,
            DataVisualType.Hexadecimal => 20
            ,
            DataVisualType.Binary => 65
        };

        #endregion Methods

        #region Events delegate

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && IsFocused && waitingForRelease == false)
            {
                //Is focused set editing to second char.
                _keyDownLabel = KeyDownLabel.SecondChar;
                UpdateCaret();
            }

            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Byte == null) return;

            if (KeyValidation(e)) return;

            //MODIFY BYTE
            if (!ReadOnlyMode && KeyValidator.IsHexKey(e.Key))
                switch (_parent.DataStringVisual)
                {
                    case DataVisualType.Hexadecimal:

                        #region Edit hexadecimal value 

                        var key = KeyValidator.IsNumericKey(e.Key)
                            ? KeyValidator.GetDigitFromKey(e.Key).ToString()
                            : e.Key.ToString().ToLower();

                        //Update byte
                        var byteValueCharArray = ByteConverters.ByteToHexCharArray(Byte.Value);
                        switch (_keyDownLabel)
                        {
                            case KeyDownLabel.FirstChar:
                                byteValueCharArray[0] = key.ToCharArray()[0];
                                _keyDownLabel = KeyDownLabel.SecondChar;
                                Action = ByteAction.Modified;
                                Byte = ByteConverters.HexToByte(
                                    byteValueCharArray[0] + byteValueCharArray[1].ToString())[0];
                                break;
                            case KeyDownLabel.SecondChar:
                                byteValueCharArray[1] = key.ToCharArray()[0];
                                _keyDownLabel = KeyDownLabel.NextPosition;

                                Action = ByteAction.Modified;
                                Byte = ByteConverters.HexToByte(
                                    byteValueCharArray[0] + byteValueCharArray[1].ToString())[0];

                                //Insert byte at end of file
                                if (_parent.Length != BytePositionInStream + 1)
                                {
                                    _keyDownLabel = KeyDownLabel.NextPosition;
                                    OnMoveNext(new EventArgs());
                                }
                                break;
                            case KeyDownLabel.NextPosition:
                                _parent.AppendByte(new byte[] { 0 });

                                OnMoveNext(new EventArgs());

                                break;
                        }

                        #endregion

                        break;
                    case DataVisualType.Decimal:

                        #region Edit decimal value 

                        if (!KeyValidator.IsNumericKey(e.Key))
                        {
                            break;
                        }
                        key = KeyValidator.IsNumericKey(e.Key)
                            ? KeyValidator.GetDigitFromKey(e.Key).ToString()
                            : 0.ToString();

                        //Update byte
                        Char[] byteValueCharArray_dec = Byte.Value.ToString("d3").ToCharArray();
                        switch (_keyDownLabel)
                        {
                            case KeyDownLabel.FirstChar:
                                byteValueCharArray_dec[0] = key.ToCharArray()[0];
                                if (int.Parse(new string(byteValueCharArray_dec)) > 255) break;
                                _keyDownLabel = KeyDownLabel.SecondChar;
                                Action = ByteAction.Modified;
                                Byte = BitConverter.GetBytes(int.Parse(new string(byteValueCharArray_dec)))[0];
                                break;

                            case KeyDownLabel.SecondChar:
                                byteValueCharArray_dec[1] = key.ToCharArray()[0];
                                if (int.Parse(new string(byteValueCharArray_dec)) > 255) break;
                                _keyDownLabel = KeyDownLabel.ThirdChar;
                                Action = ByteAction.Modified;
                                Byte = BitConverter.GetBytes(int.Parse(new string(byteValueCharArray_dec)))[0];
                                break;

                            case KeyDownLabel.ThirdChar:
                                byteValueCharArray_dec[2] = key.ToCharArray()[0];
                                if (int.Parse(new string(byteValueCharArray_dec)) > 255) break;
                                _keyDownLabel = KeyDownLabel.NextPosition;

                                Action = ByteAction.Modified;
                                Byte = BitConverter.GetBytes(int.Parse(new string(byteValueCharArray_dec)))[0];

                                //Insert byte at end of file
                                if (_parent.Length != BytePositionInStream + 1)
                                {
                                    _keyDownLabel = KeyDownLabel.NextPosition;
                                    OnMoveNext(new EventArgs());
                                }
                                break;
                            case KeyDownLabel.NextPosition:
                                _parent.AppendByte(new byte[] { 0 });

                                OnMoveNext(new EventArgs());

                                break;
                        }

                        #endregion

                        break;
                    case DataVisualType.Binary:

                        #region Edit Binary value 

                        if (!KeyValidator.IsNumericKey(e.Key)
                            || KeyValidator.GetDigitFromKey(e.Key) > 1)
                        {
                            break;
                        }
                        key = KeyValidator.IsNumericKey(e.Key)
                            ? KeyValidator.GetDigitFromKey(e.Key).ToString()
                            : 0.ToString();

                        //Update byte
                        Char[] byteValueCharArray_bin = Convert
                            .ToString(Byte.Value, 2)
                            .PadLeft(8, '0')
                            .ToCharArray();
                        switch (_keyDownLabel)
                        {
                            case KeyDownLabel.FirstChar:
                                byteValueCharArray_bin[0] = key.ToCharArray()[0];
                                _keyDownLabel = KeyDownLabel.SecondChar;
                                Action = ByteAction.Modified;
                                Byte = Convert.ToByte(new string( byteValueCharArray_bin), 2);
                                break;

                            case KeyDownLabel.SecondChar:
                                byteValueCharArray_bin[1] = key.ToCharArray()[0];
                                _keyDownLabel = KeyDownLabel.ThirdChar;
                                Action = ByteAction.Modified;
                                Byte = Convert.ToByte(new string( byteValueCharArray_bin), 2);
                                break;

                            case KeyDownLabel.ThirdChar:
                                byteValueCharArray_bin[2] = key.ToCharArray()[0];
                                _keyDownLabel = KeyDownLabel.FourthChar;
                                Action = ByteAction.Modified;
                                Byte = Convert.ToByte(new string( byteValueCharArray_bin), 2);
                                break;

                            case KeyDownLabel.FourthChar:
                                byteValueCharArray_bin[3] = key.ToCharArray()[0];
                                _keyDownLabel = KeyDownLabel.FifthChar;
                                Action = ByteAction.Modified;
                                Byte = Convert.ToByte(new string( byteValueCharArray_bin), 2);
                                break;

                            case KeyDownLabel.FifthChar:
                                byteValueCharArray_bin[4] = key.ToCharArray()[0];
                                _keyDownLabel = KeyDownLabel.SixthChar;
                                Action = ByteAction.Modified;
                                Byte = Convert.ToByte(new string( byteValueCharArray_bin), 2);
                                break;

                            case KeyDownLabel.SixthChar:
                                byteValueCharArray_bin[5] = key.ToCharArray()[0];
                                _keyDownLabel = KeyDownLabel.SeventhChar;
                                Action = ByteAction.Modified;
                                Byte = Convert.ToByte(new string( byteValueCharArray_bin), 2);
                                break;

                            case KeyDownLabel.SeventhChar:
                                byteValueCharArray_bin[6] = key.ToCharArray()[0];
                                _keyDownLabel = KeyDownLabel.EighthChar;
                                Action = ByteAction.Modified;
                                Byte = Convert.ToByte(new string( byteValueCharArray_bin), 2);
                                break;

                            case KeyDownLabel.EighthChar:
                                byteValueCharArray_bin[7] = key.ToCharArray()[0];
                                _keyDownLabel = KeyDownLabel.NextPosition;

                                Action = ByteAction.Modified;
                                Byte = Convert.ToByte(new string( byteValueCharArray_bin), 2);
                                

                                //Insert byte at end of file
                                if (_parent.Length != BytePositionInStream + 1)
                                {
                                    _keyDownLabel = KeyDownLabel.NextPosition;
                                    OnMoveNext(new EventArgs());
                                }
                                break;
                            case KeyDownLabel.NextPosition:
                                _parent.AppendByte(new byte[] { 0 });

                                OnMoveNext(new EventArgs());

                                break;
                        }

                        #endregion

                        break;
                }

            UpdateCaret();

            base.OnKeyDown(e);
        }

        #endregion Events delegate

        #region Caret events/methods

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            _keyDownLabel = KeyDownLabel.FirstChar;
            UpdateCaret();

            base.OnGotFocus(e);
        }

        private void UpdateCaret()
        {
            if (ReadOnlyMode || Byte == null)
                _parent.HideCaret();
            else
            {
                //TODO: clear size and use BaseByte.TextFormatted property...
                var size = Text[1].ToString()
                    .GetScreenSize(_parent.FontFamily, _parent.FontSize, _parent.FontStyle, FontWeight,
                        _parent.FontStretch, _parent.Foreground, this);

                _parent.SetCaretSize(Width / 2, size.Height);
                _parent.SetCaretMode(CaretMode.Overwrite);

                switch (_keyDownLabel)
                {
                    case KeyDownLabel.FirstChar:
                        _parent.MoveCaret(TransformToAncestor(_parent).Transform(new Point(0, 0)));
                        break;
                    case KeyDownLabel.SecondChar:
                        _parent.MoveCaret(TransformToAncestor(_parent).Transform(new Point(size.Width + 1, 0)));
                        break;
                    case KeyDownLabel.NextPosition:
                        if (_parent.Length == BytePositionInStream + 1)
                            if (_parent.AllowExtend)
                            {
                                _parent.SetCaretMode(CaretMode.Insert);
                                _parent.MoveCaret(TransformToAncestor(_parent).Transform(new Point(size.Width * 2, 0)));
                            }
                            else
                                _parent.HideCaret();

                        break;
                }
            }
        }

        #endregion

    }
}
