# SmoothPortals

This is a work-in-progress patch to https://github.com/KillianMcCabe/SmoothPortals
Don't use this. Use Killian's code:)

To think about:
I don't think the portals will work well for split-screen multiplayer; it might be possible to put the portals in the player template and use per-player camera culling as a hack. Per-player/maincamera rendertextures would probably be better.
I think I need some function to detect if screen resolution changes, as the rendertesxtures will need resizing.
