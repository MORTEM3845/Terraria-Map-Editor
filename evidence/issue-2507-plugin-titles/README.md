# Issue 2507 runtime evidence

Captured from the isolated diagnostic build using the Russian locale and the repository fixture world.

- `runtime-state-ru.json` records the live plugin `Name` identifiers and localized `Title` values.
- `main-window-ru.png` is the TEdit WPF window capture.
- `main-window-ru-viewport.png` is the separate MonoGame viewport capture.

The plugin submenu itself could not be captured: WPF exposed only the main window presentation source in the spider desktop. The localized values above were read directly from the running application's plugin view models.
