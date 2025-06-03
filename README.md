
# Training System

I started this Project as a Bachelor Thesis. <br><br>
This is a prototype of a VR surgery training system that will be used to compare different kinds of haptic feedback in VR. It works with Sensegloves and should work with any Steam VR compatible devices (tested with HTC Vive Pro). <br><br>
Both Versions of the prototype use the [CuttingPlaneCreation.unity](https://github.com/leon-rgb/Training-System/tree/main/VR%20Training%20System/Assets/Scenes) scene for managing Cutting Planes.

#### The structure of the of the application consists of three scenes:
![Screenshot_1](https://github.com/user-attachments/assets/ace58627-a635-4692-a60e-2c5ae8ec3032)

To chose what version you want to use, you have to change the [version.txt](https://github.com/leon-rgb/Training-System/blob/main/VR%20Training%20System/Assets/version.txt) file in this repository. This is not transfered if you make a build of the application. The version file is created when first starting and can be found in the folder /VR Surgery Training System_Data then.

### Versions, Plugins, SDKs, ...
It uses Unity Version: 2020.3.3f1 [lower versions are not recommended]. <br>**The repository uses git lfs --> should be activated**<br><br>
- [SteamVR Unity Plugin (Version 2.7.3)](https://valvesoftware.github.io/steamvr_unity_plugin/)
- [SenseGlove Unity Plugin (Version 2.2)](https://github.com/Adjuvo/SenseGlove-Unity)
- VIVE Input Utility (Version  1.15.0)
- XR Plugin Management (Version 4.0.7)
- Pro Builder (Version 4.5.2)
- Post Processing (Version 3.1.1)

*Packages that are not linked can be found directly in the Unity Package Manager or the Asset Store.*

## Windows Builds, demo videos and in detail documentation of the system can be found here:

[Google Drive Link](https://drive.google.com/drive/folders/1OyvivWRcgjhE81wN5nj4Lj1z13s1_HjY) <br>
[One Drive Link](https://1drv.ms/u/s!AkgSs2wgFvVx0CggKm4iPNe0PKfU?e=Twoape)


## Controller Version

The scene of the Controller version is "Controller_SurgeryRoom.unity" and can be found in [/VR Training System/Assests/Scenes](https://github.com/leon-rgb/Training-System/blob/main/VR%20Training%20System/Assets/Scenes/) <br><br>
Make sure to disable all controllers (including trackers) that you are not using before you open the VR scene to ensure that the haptic feedback for the controller version works properly. 



## SenseGlove Version
The scene of the SenseGlove version is "Gloves_SurgeryRoom.unity" and can be found in [/VR Training System/Assests/Scenes](https://github.com/leon-rgb/Training-System/blob/main/VR%20Training%20System/Assets/Scenes/) <br><br>
