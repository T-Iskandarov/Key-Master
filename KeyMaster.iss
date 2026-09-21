[Setup]
AppName=Key Master
AppVersion=1.0.0
AppVerName=Key Master 1.0 (Muallif: Tursunpo'lat Iskandarov - Cubo.uz)
AppPublisher=Cubo.uz (Muallif: Tursunpo'lat Iskandarov)
AppPublisherURL=https://cubo.uz
AppSupportURL=https://cubo.uz
AppUpdatesURL=https://cubo.uz
DefaultDirName={autopf}\Key Master
DefaultGroupName=Key Master
OutputDir=.\Installer
OutputBaseFilename=KeyMaster_Setup_v3
Compression=lzma2/ultra64
SolidCompression=yes
SetupIconFile=QisqaTugma\Resources\app.ico
UninstallDisplayIcon={app}\KeyMaster.exe
WizardImageFile=wizard_large.bmp
WizardSmallImageFile=wizard_small.bmp
PrivilegesRequired=lowest
DisableProgramGroupPage=yes
ShowLanguageDialog=yes

[Languages]
Name: "uz"; MessagesFile: "compiler:Languages\Uzbek.isl"
Name: "kaa"; MessagesFile: "compiler:Languages\Karakalpak.isl"
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "ru"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
Source: "Publish\KeyMaster.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\Key Master"; Filename: "{app}\KeyMaster.exe"
Name: "{autodesktop}\Key Master"; Filename: "{app}\KeyMaster.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\KeyMaster.exe"; Description: "{cm:LaunchProgram,Key Master}"; Flags: nowait postinstall skipifsilent
