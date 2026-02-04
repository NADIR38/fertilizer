; =====================================================
; Fertilizer SOP - Installer
; Company: Devinfantry
; Architecture: x64
; =====================================================

#define MyAppName "Fertilizer SOP"
#define MyAppVersion "1.5"
#define MyAppPublisher "Devinfantry"
#define MyAppURL "https://devinfantry.com"
#define MyAppExeName "fertilizesop.exe"

[Setup]
AppId={{9A31C3A8-3EB4-4E0F-8E76-4996C32D80E1}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}

DefaultDirName={autopf}\{#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExeName}

ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible

DisableProgramGroupPage=yes
PrivilegesRequired=lowest
SolidCompression=yes
WizardStyle=modern

; Optional installer protection (NOT SQL password)
; Password=Fertilizer@2026
; Encryption=yes

OutputBaseFilename=FertilizerSOP_Setup_v{#MyAppVersion}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop icon"; Flags: unchecked

; -----------------------------------------------------
; Application Files (clean & automatic)
; -----------------------------------------------------
[Files]
Source: "fertilizesop\bin\Release\*"; \
    DestDir: "{app}"; \
    Flags: recursesubdirs ignoreversion; \
    Excludes: "*.pdb"

; -----------------------------------------------------
; Shortcuts
; -----------------------------------------------------
[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

; -----------------------------------------------------
; Launch after install
; -----------------------------------------------------
[Run]
Filename: "{app}\{#MyAppExeName}"; \
Description: "Launch {#MyAppName}"; \
Flags: nowait postinstall skipifsilent
