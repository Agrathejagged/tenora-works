# Phantasy Star Universe Toolset
PSULib, the PSU Generic Parser, and other smallish derived projects

# What's Included
  - **PSULib**: DLL containing all of the file classes.
  - **PSU Generic Parser**: Generalized research/editing tool for PSU (and \_sometimes\_ the portable games). Entirely focused on individual files--no correlation is done at all (e.g while you can edit the weapon stats and you can edit the weapon star ratings, you cannot edit these two things simultaneously as they are in separate files). Primarily focused on AotI PC; while 360 archives can be read, most files in those archives cannot.
  - **Mission Builder**: Used to compile individual missions from a quest NBL and zone NBL(s).
  - **FPB Extractor**: Replacement for the gasetools fpb.exe. Allows extracting PSP "fpb" files, either using the game's mappings or ripping out every single NBL/AFS/ADX file via detection. Reference the FPB Extractor readme for details.

# Supported Data
Gameplay:
  - itemCommonInfo.xnr (view/edit): Item ID/rarity mapping file.
  - itemBulletParam\*.xnr (view/edit): Bullet Art parameter files. AOTI/PSP1 only.
  - itemSkillParam\*.xnr (view/edit): Skill parameter files. AOTI/PSP1 only.
  - itemTechParam0(1-6)\*.xnr (view/edit): Technic parameter files for standard technics. Does not include support for itemTechParam11\_EnemyB.xnr. AOTI/PSP1 only.
  - itemLineUnitParam.xnr (view/edit): Unit parameter file. AOTI/PSP1 only.
  - itemPartsParam.xnr (view/edit): CAST Parts parameter file. AOTI/PSP1 only.
  - itemSuitParam.xnr (view/edit): Clothes parameter file. AOTI/PSP1 only.
  - itemWeaponParam\*.xnr (view/edit): Weapon parameter files. AOTI/PSP1 only.
  - itemEnemyDrop.xnr (view/edit): Enemy drop tables. AOTI only.
  - itemObjectDrop.xnr (present in PSULib, no UI): Box/tree/etc drop tables. AOTI only.
  - EnemyLevelBaseParam.xnr (view/edit): Base enemy stat table.
  - Think\*Dragon.xnr (view only, very rough): AI control file for De Ragan/De Ragnus/Zoal Goug/Alteraz Gohg. Does the bare minimum of parsing, most data is still un-annotated.
  - \*Tutor.bin scripts (view/edit): Enemy scripts.
  - obj_param.xnr (rough): Set object model definitions.
  - obj_particle_info.xnr (rough): Set object particle references.

Missions:
  - enemy\_\*.xnr (view/edit): Monster layout data.
  - set\_r\*.rel (view/edit): Object layout data.
  - .bin scripts (view/edit): Mission scripts.

Aesthetic:
  - XVR textures (view/edit): Most PSU textures. Note: cannot import textures into DXT formats, these will be converted into a raster format when modified.
  - GIM/UVR textures (view only): Textures from PSP1/2/i. PSP1 mostly uses .gim, 2 and Infinity mostly use .uvr.
  - XNT texture lists (view/edit): Map model texture IDs to XVR files
  - NOM animations (view only, very rough): Player animations for PSU (PC/PS2) and PSP1/2/i. Does the bare minimum of parsing, most data is still unknown.
  - partsinfo.xnr (view/edit): Control data to map character appearance values to models in the character AFS file.

Miscellaneous:
  - .k/.bin text files (view/edit): Game text.

# TO-DO List
  - Replace the awful script editor
  - Add support for player model textures
  - Add a few other related projects

# Special Thanks
  - essen: initial research on PSU's file formats, including [gasetools](https://github.com/essen/gasetools).
  - scriptkiddie: Heavy research into PSU's data formats
  - shadowth117: Cleanup, a few missing file viewers (set editing, enemy layout editing), heavily pushing for this release

# Included code:
  - GIMSharp from [Puyo Tools](https://github.com/nickworonekin/puyotools)
  - [WpfHexEditorControl](https://github.com/abbaye/WpfHexEditorControl)
