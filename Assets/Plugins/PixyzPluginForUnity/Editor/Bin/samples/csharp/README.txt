#install dotnet https://docs.microsoft.com/fr-fr/dotnet/core/install/linux-debian

#mkdir ~/.nuget/NuGet -p
#cp NuGet.Config ~/.nuget/NuGet
#mkdir ~/.private_nuget -p
#cp PiXYZsdk.2020.2.2.21.nupkg ~/.private_nuget

#edit NuGet.Config -> replace $USER by good one

#in sample folder
dotnet build
dotnet run

