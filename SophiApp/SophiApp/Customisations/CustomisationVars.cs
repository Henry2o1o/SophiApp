﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SophiApp.Customisations
{
    internal struct CustomisationVars
    {
        internal const string _800_MSI_EXTRACT = @"Msi.Package\shell\Extract";
        internal const string _800_MSI_EXTRACT_COMMAND = @"Msi.Package\shell\Extract\Command";
        internal const string _800_MSI_EXTRACT_VALUE = "msiexec.exe /a \"%1\" /qb TARGETDIR=\"%1 extracted\"";
        internal const string MUIVERB = "MUIVerb";
        internal const string _800_MSI_MUIVERB_VALUE = "@shell32.dll,-37514";
        internal const string _800_MSI_ICON = "Icon";
        internal const string _800_MSI_ICON_VALUE = "shell32.dll,-16817";
        internal const string _801_CAB_COMMAND = @"CABFolder\Shell\RunAs\Command";
        internal const string _801_CAB_RUNAS = @"CABFolder\Shell\RunAs";
        internal const string _801_CAB_VALUE = "cmd /c DISM.exe /Online /Add-Package /PackagePath:\"%1\" /NoRestart & pause\"";
        internal const string _801_MUIVERB_VALUE = "@shell32.dll,-10210";
        internal const string _801_CAB_LUA_SHIELD = "HasLUAShield";
        internal const string _802_EXTENDED = @"Extended";
        internal const string _802_RUNAS_USER = @"exefile\shell\runasuser";
        internal const string _803_CAST_TO_DEV_GUID = @"{7AD84985-87B4-4a16-BE58-8B72A5B390F7}";
        internal const string _803_CAST_TO_DEV_VALUE = "Play to menu";
        internal const string _804_SHARE_GUID = @"{E2BF9676-5F8F-435C-97EB-11607A5BEDF7}";
        internal const string _805_MS_PAINT_3D = "Microsoft.MSPaint";
        internal const string _806_BMP_EXT = @"SystemFileAssociations\.bmp\Shell\3D Edit";
        internal const string _807_GIF_EXT = @"SystemFileAssociations\.gif\Shell\3D Edit";
        internal const string _808_JPE_EXT = @"SystemFileAssociations\.jpe\Shell\3D Edit";
        internal const string _809_JPEG_EXT = @"SystemFileAssociations\.jpeg\Shell\3D Edit";
        internal const string _810_JPG_EXT = @"SystemFileAssociations\.jpg\Shell\3D Edit";
        internal const string _811_PNG_EXT = @"SystemFileAssociations\.png\Shell\3D Edit";
        internal const string _812_TIF_EXT = @"SystemFileAssociations\.tif\Shell\3D Edit";
        internal const string _813_TIFF_EXT = @"SystemFileAssociations\.tiff\Shell\3D Edit";
        internal const string _814_PHOTOS_SHELL_EDIT = @"AppX43hnxtbyyps62jhe9sqpdzxn1790zetc\Shell\ShellEdit";
        internal const string _815_PHOTOS_SHELL_VIDEO = @"AppX43hnxtbyyps62jhe9sqpdzxn1790zetc\Shell\ShellCreateVideo";
        internal const string _816_IMG_SHELL_EDIT = @"SystemFileAssociations\image\shell\edit";
        internal const string _817_BAT_SHELL_EDIT = @"batfile\shell\print";
        internal const string _817_CMD_SHELL_EDIT = @"cmdfile\shell\print";
        internal const string _818_SHOW_VALUE = "{3dad6c5d-2167-4cae-9914-f99e41c12cfa}";
        internal const string _818_HIDE_VALUE = "-{3dad6c5d-2167-4cae-9914-f99e41c12cfa}";
        internal const string _818_LIB_LOCATION = @"Folder\ShellEx\ContextMenuHandlers\Library Location";
        internal const string _819_SHOW_VALUE = "{7BA4C740-9E81-11CF-99D3-00AA004AE837}";
        internal const string _819_HIDE_VALUE = "-{7BA4C740-9E81-11CF-99D3-00AA004AE837}";
        internal const string _819_SEND_TO = @"AllFilesystemObjects\shellex\ContextMenuHandlers\SendTo";
        internal const string _820_BITLOCKER_BDE_ELEV = @"Drive\shell\encrypt-bde-elev";
        internal const string _820_WIN_VER_ENT = "Enterprise";
        internal const string _820_WIN_VER_PRO = "Professional";
        internal const string _821_BMP_ITEM_NAME = "ItemName";
        internal const string _821_BMP_NULL_FILE = "NullFile";
        internal const string _821_BMP_SHELL_NEW = @".bmp\ShellNew";
        internal const string _822_MS_WORD_PAD = "Microsoft.Windows.WordPad";
        internal const string _822_RTF_DATA = "Data";
        internal const string _822_RTF_ITEM_NAME = "ItemName";
        internal const string _822_RTF_SHELL_NEW = @".rtf\ShellNew";
        internal const string _823_ZIP_SHELL_NEW = @".zip\CompressedFolder\ShellNew";
        internal const string _824_CURRENT_EXPLORER = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer";
        internal const string _824_PROMPT_MIN = "MultipleInvokePromptMinimum";
        internal const int _824_PROMPT_MIN_VALUE = 300;
        internal const string _825_NO_USE_STORE = "NoUseStoreOpenWith";
        internal const string _825_SOFTWARE_EXPLORER = @"SOFTWARE\Policies\Microsoft\Windows\Explorer";
        internal const string CAPABILITY_MS_PAINT = "Microsoft.Windows.MSPaint";
        internal const string PROGRAM_ACCESS_ONLY = "ProgrammaticAccessOnly";
        internal const string SHELL_EXT_BLOCKED = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked";
        internal const string UWP_MS_WIN_PHOTOS = "Microsoft.Windows.Photos";
    }
}
