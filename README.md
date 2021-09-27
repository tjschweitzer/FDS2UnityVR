# Fast Fuels Unity VR Readme

## After Opening package in unity 
- One asset needs to be downloaded and imported [https://assetstore.unity.com/packages/2d/textures-materials/nature/terrain-textures-pack-free-139542]
- In Package Manager 
	- Change to Unity Registry 
	- Find "Input System" Download, Import and then Restart Unity
## Settings that need to be changed
- Edit - > Project Settings - > Player -> Configuration  - > Api Compatibility Level* -> .NET4.x
- Edit - > Project Settings - > Player -> Configuration  - > Active Input Handling -> Both

# How to load different FDS Files
1. In Hierachy Expand The Scene "FireVR" (this may also be found in the Scenes folder in the Project window)
1. Expand MainController
1. Select ConfigData
	- File Name : Full Path to the .fds input file
		- e.g. `treemodlist.fds`
	- pl3d Data Dir : Directory where the plot3d JSON's are located
	- FastFuels File Name : Full path to Json Output from Standfire
		- e.g. `newloc.json`
	- StandFire Json File Name : Full path to Json Output from Standfire  
		- e.g. `newloc.json`


### Goal List

- [x] Load In Fast Fuels JSON
- [x] Load In Topography
- [x] Teleportation Movement
- [x] Flight Movement 



