dotnet publish crypto.Desktop.Console -c Release -o ausfuehrbare/windows/win-x64/NeniCrypto --self-contained true -r win-x64
dotnet publish crypto.Desktop.Console -c Release -o ausfuehrbare/windows/win-x86/NeniCrypto --self-contained true -r win-x86

dotnet publish crypto.Desktop.Console -c Release -o ausfuehrbare/windows/win7-x64/NeniCrypto --self-contained true -r win7-x64
dotnet publish crypto.Desktop.Console -c Release -o ausfuehrbare/windows/win7-x86/NeniCrypto --self-contained true -r win7-x86

dotnet publish crypto.Desktop.Console -c Release -o ausfuehrbare/linux-x64/NeniCrypto --self-contained true -r linux-x64