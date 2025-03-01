# Shakespeare 3D

**Shakespeare 3D** is an interactive Unity project that brings Shakespearean drama to life in a fully 3D environment. 
Users can edit scenes, position characters, select scripts and experience guided tutorials—all inspired by the timeless works of Shakespeare.

## Project Overview

Shakespeare 3D is designed to:
- **Visualize Scenes in 3D**: Display characters and settings on a virtual stage.
- **Script Selection & Playback**: Allow users to choose from multiple scripts loaded from external text files and see the scene animate accordingly.
- **Interactive Scene Editing**: Enable users to drag and drop characters on the stage and control their movements.
- **Undo/Redo Functionality**: Let users undo or redo character movements before finalizing a scene.
- **Tutorial/Instructions Overlay**: Provide a step-by-step tutorial that guides users through the key functionalities of the project.

## Key Features

- **User Interface (UI) Management**:  
  - An intuitive UI for repositioning characters, editing scenes, and controlling animations.
  - Drag-and-drop functionality for a seamless editing experience.

- **Script Selection**:  
  - Script details are loaded from external text files in the Resources folder.
  - A modal pop-up UI allows users to cycle through and select a script.
  - Dynamic updating of UI elements to reflect the selected script.

- **Camera Control**:  
  - Supports multiple camera modes (Follow, Isometric, Free, TopDown).
  - Smooth camera movement and rotation with configurable sensitivity.
  - Disables background zoom/scroll when UI overlays (like script selection) are active.

- **Undo/Redo System**:  
  - Records character movements so that users can easily undo/redo actions before saving.
  - Integrates with UI buttons to enable/disable actions based on user interactions.

- **Scene Saving & Loading**:  
  - Saves scene configurations (characters’ positions, selected scripts) as JSON files.
  - Loads saved scenes for playback or further editing.

- **Tutorial/Instructions Overlay**:  
  - A guided tutorial using a series of overlay panels with arrows and descriptions.
  - Users can step through the tutorial with Next/Skip buttons.
  - The overlay disables background interactions until the tutorial is complete.
- **Animation**:
  - 
## Getting Started

### Prerequisites

- Unity 2020.3 or later.
- TextMeshPro package (included with Unity via the Package Manager).
- [Optional] DOTween for additional tweening animations.

### Installation

1. **Clone or Download the Repository**  
   Clone the repository or download the project files.

2. **Open the Project in Unity**  
   Open the project folder in your version of Unity.

3. **Add Scenes to Build Settings**  
   Go to **File → Build Settings** and ensure the following scenes are added:
   - Main Menu
   - New Scene
   - Instructions Scene (if using a separate scene for instructions)

4. **Import Required Assets**  
   Make sure all necessary assets (fonts, models, scripts, etc.) are imported into the appropriate folders:
   - **Assets/Fonts**
   - **Assets/Models**
   - **Assets/Resources** (for text files and other resources)

### Running the Project

1. **Main Menu**:  
   - Start from the Main Menu scene.
   - Click **New Scene** to proceed.

2. **Tutorial Overlay**:  
   - When you enter the New Scene, a tutorial overlay will guide you through key features (script selection, character movement, save functions, etc.).
   - Use the **Next** button to go through the steps or **Skip** to bypass the tutorial.

3. **Scene Editing**:  
   - Once the tutorial is finished, the script selection UI will open.
   - You can then interact with the scene by selecting scripts, moving characters, and saving your work.
     
4. **Load Scene**:
   - After you save your work, you will be able to aceess your file in the load scene.
   - You can edit & play your work

## Project Structure

- **Assets/Scripts**: Contains all project scripts including:
  - `CameraController.cs`
  - `ScriptSelectionManager.cs`
  - `UndoRedoManager.cs`
  - `TutorialManager.cs`
- **Assets/Fonts**: Contains imported fonts and related meta files.
- **Assets/Models**: Contains 3D models for characters and stage.
- **Assets/Resources**: Contains text files (e.g., `ScriptDetail1.txt`, etc.) and other resource files.
- **Assets/Scenes**: Contains Unity scenes (Main Menu, New Scene, Instructions Scene).

## Known Issues & Future Improvements

- **UI Interactions**: Further refinement may be needed to ensure all background interactions are disabled during tutorial overlays.
- **Error Handling**: Additional error checks and user feedback mechanisms can be implemented.
- **Enhanced Animations**: Future updates may include more dynamic animations and transitions for both UI elements and 3D scene interactions.
- **User Customization**: Options for users to customize the interface and interactions will be considered.

## Credits

- **Developed by**: Farah Hadhirah Jaafar, Aaron Ng Kian Kiat, Quek Zi Ying, Sharifah Fadilah Syed Azlan, Pan Hao & Engku Thaqif Syahmi
- **Inspiration**: Shakespeare’s works and interactive storytelling.
- **Third-Party Assets/Libraries**:  
  - TextMeshPro (Unity Technologies)


## License

[Specify your project's license here, e.g., MIT License, if applicable.]

