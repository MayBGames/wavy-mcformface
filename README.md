# Hello there!

I'm Wavy McFormface, and I'm here to help you make awesome procedural sounds and music at runtime :stuck_out_tongue_winking_eye:

# Overview

Wavy McFormface is an analog synth that allows to to create and waveforms in real time.

Wavy offers a wealth of controls to edit and constomize your waveform. And, as if that weren't enough, you can also edit your waveform at runtime using Wavy's api!

Below we'll step through the install process, how to create a waveform, the parts of a waveform, and how to use the editor ui and api to edit/modify your waveform.

# Install Wavy

This is pretty simple - all you need is the `Wavy McFormface/Assets/Wavy McFormface` directory at the root of your project. I realize the directory structure is weird - it's required by Unity. The most important directory in this entire project is the `Editor` directory. The `Editor` directory is special; Unity looks inside it for ui widgets and such. One pattern you could use to get Wavy ready to go is:

```
git clone git@github.com:bsgbryan/wavy-mcformface.git # Clones project to ./wavy-mcformface
cp -R ./wavy-mcformface/Assets/Wavy\ McFormface {YOUR_PROJECT_ROOT}
```

This will put the `Wavy McFormface` directory in your project's root directory - which is exactly what we want.

You may need to restart Unity for it to pick up that Wavy was added to the project.

# Create your first waveform!

### TL;DR

* __Tools -> Wavy McFormface -> New Waveform__ (or _CTRL + ALT + SHIFT + W_)
* Add `Wavy Mc Formface` component to `GameObject` in scene
* Add `Audio Source` component to same `GameObject` Wavy was added to

### Details

To create a waveform simply select the *Tools -> Wavy McFormface -> New Waveform* Menu (or use the _CTRL + ALT + SHIFT + W_ shortcut) and select where you'd like your waveform config to be saved. I'd recommend something like a `Waveforms` directory in your project root.

Once you click `Save` navigate to the created file. Note that a `.asset` file was created. This is important as assets files can be shared across multiple instances of Wavy McFormface scripts.

To start editing your waveform, simply create a new `GameObject` or find the existing `GameObject` you want and click *Add Component*. Then find the "Wavy Mc Formface" component and click it. Be sure to add an `Audio Source` component to your `GameObject` as well - without an `Audio Source` Wavy has no way to produce sound.

## Scripts vs Assets

What is the difference between a script and an asset?

### Assets

Wavy McFormface assets are called Configies and hold all the configuration data for a waveform. All the edits and update you make via the editor ui or api are maintained in the Configie.

Why? Because assets are not scene specific and changes to them persist during Play Mode. This is important because your scene has to be playing for the `OnAudioFilterRead` Unity api method to be called. The `OnAudioFilterRead` method is the hook Wavy uses to generate waveforms. You can read about `OnAudioFilterRead` [here](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnAudioFilterRead.html "Unity Documentation").

Since the scene needs to be in Play Mode for Wavy to generate waveforms we really need to our edits to waveform Configies to persist beyond Play Mode. Assets are the best way to achieve this. Since an asset exists outside a scene it gets updated whether Unity is in Play Mode or not. Using assets for Configies means that you can enter Play Mode and get your waveform sounding exactly the say you want. When you exit Play Mode all your work will persist because it was saved against the Configie - not the GameObject the script is attached to.

### Scripts

Scripts are the logic that that uses a Configie to generate a a waveform. Scripts get attached to `GameObjects` and require an `Audio Source` also be attached to the same `GameObject` as the script.

A script references a Configie (as is shown in the Configie and Master Volume section below).

Importantly, you can edit a Configie either by editing the asset directly, or by using the script - both editors do the same thing. Most of the time you'll likely use the script editor ui to edit the Configie while in Play Mode.

It's important to node that you can have multiple scripts reference the same Configie. In this case, a change made via the script editor ui will effect all script instances referencing the same Configie.

# The Editor ui

There are several sections of the editor ui, each with their own purpose. The following sections will go through each section in turn.

## Configie and Master Volume

![Configie and Master Volume editor ui section]("/Screenshots/Configie and Master Volume.png?raw=true" "Configie and Master Volume editor ui section")
