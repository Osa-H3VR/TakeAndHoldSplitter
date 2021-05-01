# TakeAndHoldSplitter

## Requires:
- LiveSplit with:
  - LiveSplitServer component installed (https://github.com/LiveSplit/LiveSplit.Server)
- A split file with compatible split segments (example is provided here)
- A layout with LiveSplitServer component 

## Installation
1. Just like any `.deli` mod, just put it into `H3VR\Deli\mods`
2. It should be working, but if you have a custom LiveSplit server setup, use `H3VR\Deli\configs\osa.takeandholdsplitter.cfg` to setup it correctly.

## Usage
1. Add LiveSplitServer component to you Layout
2. Use `Controls->Start server` to start the server before the run.

# Building
1. Locate `Assembly-CSharp.dll` in H3VR dir
2. Publicize it, however you find it fit
2. Use https://github.com/MonoMod/MonoMod/releases to generate hook .dll (use net35!) aka. `MonoMod.RuntimeDetour.HookGen.exe --private [path to assembly csharp]`
3. Put both in `dependencies` folder
