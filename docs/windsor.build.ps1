nuget install Castle.Windsor -Version 4.0.0
cp .\Castle.Windsor.4.0.0\lib\net45\* .
dotnet run --project ../CSharpFormatting/CSharpFormatting.Cli/CSharpFormatting.Cli.csproj -- -r ./windsor.md -o true -f .