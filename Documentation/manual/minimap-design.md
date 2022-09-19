# Minimap Design

This document presents a basic overview of how the minimap is designed
for the unity project.

## Requirements

The minimap can support a few different scenarios and
requirements, let's define those at the start of
this document.

1. Show player's surroundings.
1. Allow for easy navigation of virtual space.
1. Highlight relevant information for a user.
1. Allow player to focus on minimap to find important information.

## Examples

This is a very broad definition of a minimap but there are a few
existing high level minimap designs that could be derived from this.

The simplest minimap may be something that just shows
the room the player is located in relative to
previously explored rooms like in the game
super metroid.

![Screenshot of typical gameplay from Super Metroid.
The protagonist uses a grapple beam to reach inaccessible areas.](../resources/Super_Metroid_Grapple_Beam.png)

_Image from
[Metroid Recon](http://metroid.retropixel.net/games/metroid3/screenshots/),
[Wikipedia Source Details](https://en.wikipedia.org/wiki/File:Super_Metroid_Grapple_Beam.png)_

A more complex version of this minimap design may be something similar to
Star craft 2's minimap which shows fog of war, terrain, units, as well
as other important features and has navigation controls.

![StarCraft 2 game Outbreak mission in Wings of liberty campaign.](../resources/starcraft-screenshot.jpg)

## Features

From the given requirements and examples above, we can define a set of
features that satisfy these requirements.

## Design Overview

The minimap needs to be composed of a few components and layers composited together.

```mermaid
flowchart TB

subgraph minimap

```
