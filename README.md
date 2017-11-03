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

Scripts are the logic that that uses a Configie to generate a waveform. Scripts get attached to `GameObjects` and require an `Audio Source` also be attached to the same `GameObject` as the script.

A script references a Configie (as is shown in the Configie and Master Volume section below).

Importantly, you can edit a Configie either by editing the asset directly, or by using the script - both editors do the same thing. Most of the time you'll likely use the script editor ui to edit the Configie while in Play Mode.

It's important to note that you can have multiple scripts reference the same Configie. In this case, a change made via the script editor ui will effect all script instances referencing the same Configie.

# The Editor ui

There are several sections of the editor ui, each with their own purpose. Below we go through each section in turn.

## Overview

Wavy McFormface has five types of properties:

* AnimationCurve
* Float
* Integer
* Range
* Time

AnimationCurve, Float, Range, and Integer property types may be edited in real time while in Play Mode. Time properties may not be edited in real time in Play Mode.

AnimationCurve properites are:

* Envelope attack and decay curves
* Harmonics
* Frequency Limit
* Wibble Wobble pattern curve
* Noise pattern curve

Float properties are:

* Master Volume
* Sine, Square, Sawtooth, and Triangle Wave Volumes
* Noise level

The single Integer property is:

* Play Controls current octave

The two Range properties are:

* Play Controls octave range
* Noise variance

The Envelope attack/decat times as well as the Wibble Wobble Time fields are all Time values and may not be edited in real time while in Play Mode.

## Configie and Master Volume

![Configie and Master Volume editor ui section](/Screenshots/Configie%20and%20Master%20Volume.png?raw=true "Configie and Master Volume editor ui section")

**Configie _may not_ be updated in real time while in Play Mode**

**Master Volume _may_ be updated in real time while in Play Mode**

The top-most section is where you specify which Configie a script is using, and set the Master Volume.

To change which Configie a script uses, simply select a different one from the component chooser.

Wavy McFormface supports all four basic wave types:

* Sine
* Sawtooth
* Square
* Triangle

The volume level for each wave type can be specified individually. The Master Volume raises or lowers all wave type volumes uniformly, and can be used as a cutoff to make sure the overall volume output nevers get beyond a desired threshold.

_**IMPORTANT NOTE**_ The editor ui enforces a range of 0.0 - 1.0 for the value of the Master Volume. The api, because of the way Unity works, does not. **Do not ever specify a Master Volume level above 1.0** as doing so may cause damage to your audio equipment and/or your ears. This is extrmeely serious.

## Wave Volumes

![Wave Volumes editor ui section](/Screenshots/Wave%20Volumes.png?raw=true "Wave Volumes editor ui section")

**These values _may_ be updated in real time while in Play Mode**

This section allows you specify volume levels for all four wave types independently - play around with these to create something awesome and unique!

_**IMPORTANT NOTE**_ The editor ui enforces a range of 0.0 - 1.0 for each of these values. The api, because of the way Unity works, does not. **Do not ever specify a wave volume level above 1.0** as doing so may cause damage to your audio equipment and/or your ears. This is extrmeely serious.

## Play Controls

![Play Controls editor ui section](/Screenshots/Play%20Controls.png?raw=true "Play Controls editor ui section")

This section looks complex, but it's really just three things:

1. The octave range specifier
2. The current octave slider
3. The note grid

We'll go through each item in turn:

### octave range

**This value _may_ be updated in real time while in Play Mode**

The octave range specifies all octaves to render when Harmonics are active.

Wavy can generate a pure tone (a single note in a single octave) or it can generate multiple tones that play at once. Generating multiple tones that play at once typically sounds more natural and pleasent. Wavy gives you the freedom to select the range of octaves to render for Harmonics - giving you tremendous power to shape the sound and texture of your waveform.

_If Harmonics are not active this value is not used._

### current octave

**This value _may_ be updated in real time while in Play Mode**

This slider selects the octave to use when generating your waveform. It is bounded by the values selected in the octave range.

### note grid

The note grid is a collection of buttons allowing you to play your waveform to see how it sounds.

#### tap

The `tap` buttons roughly simulate tapping a key on a keyboard to play your waveform. The formula used to determine how long to play when tapping is as follows: `attack + attack`. The note is played for twice the attack time - this provides enough time for the note to get to full volume, with a sustain equal to the attack time. When `attack + attack` time has passed, the note stops playing and the `decay` takes over. The note volume fades out according to the `decay` time and `AnimationCurve` specified.

### play

The `play` buttons simulates pressing and holding a key on a keyboard to play your waveform. The `attack` time and `AnimationCurve` are used to get the waveform to full volume. The waveform then continues playing until `stop` is pressed. When `stop` is pressed the `decay` time and `AnimationCurve` are used to lower the waveform's volume to 0.

_**IMPORTANT NOTE**_ Only one note can play at a time. Pressing `play` on multiple notes will simply transition from the first note to the second - even though both buttons will say `stop`, only the most recent note selected will be playing.

## Envelope

![Envelope editor ui section](/Screenshots/Envelope.png?raw=true "Envelope editor ui section")

**attack/decay AnimationCurves _may_ be updated in real time while in Play Mode**

**attack/decay time values _may not_ be updated in real time while in Play Mode**

### time values

The `attack` and `decay` time values specify how long their associated actions take to complete. Because of the way the waveform is generated at runtime, these values may not be edited while in Play Mode.

### AnimationCurves

The `attack` and `decay` AnimationCurves give you absolute control over exactly _how_ a waveform's volume raises and lowers over the time specified. The AnimationCurves may be edited in real time while in Play Mode - however, changes to `attack` will not be heard until the next `play` and changes to `decay` will only be heard when `stop` is pressed.

## Harmonics

![Harmonics without details editor ui section](/Screenshots/Harmonics%20no%20details.png?raw=true "Harmonics without details editor ui section")

**These values _may_ be updated in real time while in Play Mode**

The Harmonics AnimationCurves allow you to specify how loud harmonic octaves should be relative to the current octave.

The grey bars behind the AnimationCurves show you where each rendered octave is at - making it easy to visally get an idea how loud each octave is.

### Hamronic details

![Harmonics showing details editor ui section](/Screenshots/Harmonics%20with%20details.png?raw=true "Harmonics showing details editor ui section")

The Details section provides info on the volume level for each octave below and above the current octave.

_**IMPORTANT NOTE**_ The volume range for harmonic octaves is bounded to 0.0 - 1.0. Since harmonic volumes are just AnimationCurves, this can be changed via the api. **Do not change the default volume bounds for harmonic octaves. Doing so may damage your audio equipliment and damage your ears.**

## Frequency Limit

![Frequency Limit without details editor ui section](/Screenshots/Frequency%20Limit%20no%20details.png?raw=true "Frequency Limit without details editor ui section")

**This AnimationCurve _may_ be updated in real time while in Play Mode**

The Frequency Limit curve is like an advanced volume control for all rendered frequencies of your waveform.

The grey bars behind the AnimationCurve show you where each rendered octave is at - making it easy to visally get an idea how loud each octave is.

Using the AnimationCurve you can control the volumes of every octave and note rendered in your waveform to a high degree of precision.

![Frequency Limit showing details editor ui section](/Screenshots/Frequency%20Limit%20with%20details.png?raw=true "Frequency Limit showing details editor ui section")

The Frequency Limit details shows the exact volume level of each rendered octave.

The volume range is between 0.0 (_completely silent_) and 1.0 (_full volume_).

## Wibble Wobble

This one is fun :metal:

![Wibble Wobble editor ui section](/Screenshots/Wibble%20Wobble.png?raw=true "Wibble Wobble editor ui section")

The official name for this control in synth parlance is Pitch Bending, or portamento (usually using a [Pitch Wheel(https://en.wikipedia.org/wiki/Pitch_wheel "Wikipedia page")) - but I didn't know that when I was building this plugin so I called it Wibble Wobble (which is more fun anyway).

The Wibble Wibble uses an AnimationCurve to specify how to bend you're waveform's pitch. The bend pattern repeats according the the `Time` units specified.

## Noise

Ok... this one's a doozy :grin:

![Noise editor ui section](/Screenshots/Noise.png?raw=true "Noise editor ui section")

The Noise section has three parts:

* variance
* level
* pattern (_the AnimationCurve_)

We'll go through each part in reservse order as that will make more sense:

#### pattern

This AnimationCurve defines the pattern by which a waveform should deviate from it's note over the course of a sample. This `pattern` will repeat for each call to `OnAudioFilterRead` (ie: _sample_). While this `pattern` does give shape and texture to your waveform, it is not the same as the Wibble Wobble pattern. This `pattern` is for providing suble texture changes and nuances in your waveform.

#### level

The `level` determines to what extent the `pattern` and `variance` distort the waveform. A value of 0 means the no distortion occurs. As the value increases, the waveform distortion becomes more intense.

#### variance

This range defines how far randomized noise points may deviate from the specified `pattern`. The "noise" provided by this control is a function of random deviations from the specified `pattern`, amplified by `level`.

A narrow `variance` range means the injected noise will only deviate slightly from the specified `pattern`. A wide `variance` means that more random noise deviations will be applied to the waveform as they will fall within the specified `varaince` range.

Additionally, you can control the tone of the `variance` by sliding the range up and down.

# api

All properties available through the editor ui are also available via the api. For examples of api use, please see the `Assets/Wavy McFormface/Examples` directory (specifically the [Scripts](/Assets/Wavy%20McFormface/Examples/Scripts) directory). These scenes are playable. To run them you will need the [Standard Assets](https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-32351) package installed.

## Lerping

There are two different speeds that Wavy lerps at when animating between property values:

* Slow: This lerp takes one full second to complete
* Fast: This lerp takes one full sample pass to complete (_1/48th of a second on desktop, 1/24th of a second on mobile_)

All AnimationCurve properties lerp at the slow speed. This is to prevent audio artifacts like popping - which are caused by changing values too quickly.

All animatiable non-AnimationCurve properties lerp at the fast speed. This is because changes in these values can be animated faster without causing artifacts.