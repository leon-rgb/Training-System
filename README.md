
# Training System

## Overview
This repository contains a prototype VR surgery training system developed as a Bachelor Thesis. The core idea is to let instructors define arbitrary “cutting planes” on a virtual bone model, then allow trainees to practice sawing along those planes in VR while collecting accuracy metrics. Two input methods are supported:
- **Controller Version:** Uses standard VR controllers (e.g., HTC Vive Pro) for basic vibration feedback.
- **SenseGlove Version:** Uses SenseGlove DK1 haptic gloves for richer force and vibrotactile feedback.
Both Versions of the prototype use the [CuttingPlaneCreation.unity](https://github.com/leon-rgb/Training-System/tree/main/VR%20Training%20System/Assets/Scenes) scene for managing Cutting Planes.

A separate non-VR scene is provided for creating and managing cutting planes that are stored for later use in the VR simulator.

#### The structure of the of the application consists of three scenes:
![Screenshot_1](https://github.com/user-attachments/assets/ace58627-a635-4692-a60e-2c5ae8ec3032)

To chose what version you want to use, you have to change the [version.txt](https://github.com/leon-rgb/Training-System/blob/main/VR%20Training%20System/Assets/version.txt) file in this repository. This is not transfered if you make a build of the application. The version file is created when first starting and can be found in the folder /VR Surgery Training System_Data then.

## Core Functionality
1. **Cutting Plane Creation (Non-VR Scene)**
   - Instructors place three control spheres on a bone model to define a closed cutting curve.
   - The system extrapolates this curve into a full 3D cutting surface (plane).
   - Multiple planes can be created, visualized, and saved for reuse in VR sessions.
   - Scene name: `CuttingPlaneCreation.unity`

2. **VR Training Simulation**
   - Loads a selected cutting plane into the VR environment (either controller or SenseGlove version).
   - Trainees use a virtual saw to cut along the predefined plane.
   - Real-time metrics are computed on each slice, including:
     - Number of times cut too deep
     - Maximum depth overshoot
     - Percentage of cut segments within the target plane
   - Metrics are displayed on a virtual whiteboard during the session and saved to disk for post-session analysis.

3. **Input & Haptic Feedback**
   - **Controller Version:**  
     - Scene: `Controller_SurgeryRoom.unity` can be found in [/VR Training System/Assests/Scenes](https://github.com/leon-rgb/Training-System/blob/main/VR%20Training%20System/Assets/Scenes/) 
     - Uses SteamVR controllers for positional tracking and basic vibration feedback.  
     - Ensure only the active controllers/trackers are enabled before entering the VR scene.
   - **SenseGlove Version:**  
     - Scene: `Gloves_SurgeryRoom.unity` can be found in [/VR Training System/Assests/Scenes](https://github.com/leon-rgb/Training-System/blob/main/VR%20Training%20System/Assets/Scenes/)
     - Uses SenseGlove DK1 for force-feedback and vibrotactile cues.  
     - Provides a higher level of immersion but requires the SenseGlove SDK and hardware to be connected and configured.

4. **Metrics & Data Logging**
   - During each trial, the system tracks:
     - Total cut length vs. planned plane
     - Depth deviations (too shallow or too deep)
     - Time to complete the cut
   - Results are rendered on a virtual whiteboard for immediate feedback and stored as log files (CSV or JSON) in the application’s data folder.
   - Researchers can use these logs to compare user performance across different haptic feedback conditions.

## Versions, Plugins, SDKs, ...
It uses Unity Version: 2020.3.3f1 [lower versions are not recommended]. <br>**The repository uses git lfs --> should be activated**<br><br>
- [SteamVR Unity Plugin (Version 2.7.3)](https://valvesoftware.github.io/steamvr_unity_plugin/)
- [SenseGlove Unity Plugin (Version 2.2)](https://github.com/Adjuvo/SenseGlove-Unity)
- VIVE Input Utility (Version  1.15.0)
- XR Plugin Management (Version 4.0.7)
- Pro Builder (Version 4.5.2)
- Post Processing (Version 3.1.1)

*Packages that are not linked can be found directly in the Unity Package Manager or the Asset Store.*

## Findings of the thesis / study
In a user study comparing the two input methods, haptic gloves significantly enhanced immersion, but controller‐based feedback scored higher on usability and imposed a significantly lower mental and physical workload.
A medical expert validated the system’s feasibility for orthopedic training, confirming it can support practical surgical skill development.

For more details have a look at my [complete thesis publication](https://elib.uni-stuttgart.de/items/106c6382-7ea1-43da-bc6c-8a3ff353131c)
