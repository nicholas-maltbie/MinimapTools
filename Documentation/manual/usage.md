# Usage

The MinimapTools package is composed of a few main components.

1. World space for the minimap defined via an @nickmaltbie.MinimapTools.Minimap.Shape.IMinimapShape
    * The behaviour @nickmaltbie.MinimapTools.Minimap.Shape.MinimapBoundsSource
        defines a @nickmaltbie.MinimapTools.Minimap.Shape.MinimapSquare that can
        be read via the @nickmaltbie.MinimapTools.Minimap.AbstractMinimap behaviour.

1. A minimap UI element defined as @nickmaltbie.MinimapTools.Minimap.IMinimap
    There are a few concrete implementations of this defined in the project.
    * @nickmaltbie.MinimapTools.Minimap.Centered.CenteredMinimap -
        A Centered minimap that will move and or rotate with a selected minimap
        icon from a tag. Also supports scaling the map to be larger or smaller.
    * @nickmaltbie.MinimapTools.Minimap.Simple.SimpleStaticMinimap -
        This is a simple minimap that will draw the background and objects
        on the minimap.

1. Minimap Elements that are in a fixed position on the minimap. The
    interface for this is defined as @nickmaltbie.MinimapTools.Background.IMinimapElement
    * @nickmaltbie.MinimapTools.Background.BoxMinimapElement -
        Box minimap that will draw a solid box on the background at a given
        position, rotation, and size.
    * @nickmaltbie.MinimapTools.Background.SpriteMinimapElement -
        Sprite minimap element that will draw a given texture on the
        background at a given position, rotation, and size.

1. Minimap Icons that move around on top of the minimap. The interface
    for icons is defined as  @nickmaltbie.MinimapTools.Icon.IMinimapIcon
    * @nickmaltbie.MinimapTools.Icon.FixedSizeSpriteIcon -
        Icon that will always have the same pixel size on the minimap no matter
        the zoom level.
    * @nickmaltbie.MinimapTools.Icon.RelativeSizeIcon -
        Icon that will scale with the map to maintain a consist size
        with its world size.

## Sample

The sample MinimapFPS defined at `Assets\Samples\MinimapFPS` has an
example of the minimap as part of the UIManager prefab for minimaps
at the path `Assets\Samples\MinimapFPS\UI\Minimap`.
