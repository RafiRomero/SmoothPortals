# SmoothPortals

This is a work-in-progress patch to https://github.com/KillianMcCabe/SmoothPortals
Don't use this. Use Killian's code:)

TODO:
It's probably better to walk up the heirarchy and grab the MainCamera (perhaps by tag) in Start(), rather than having to assign it manually.
This would allow you to just drop a prefab into your scene and use it as-is, without having to configure it at all.

I don't like using the Player tag to ID the PortalUser (it prevents me using other tags); a layer would be less intrusive (I can assign multiple layers), but not really what they're intended for. Or, don't us any of these:
Sender.cs can climb the heirarchy from the collider looking for a portaluser script. This works fine unless an object has multiple colliders: we only want one of any given object's colliders to act as a trigger. The portaluser script needs to live in the root of the object we want to teleport, so perhaps tagging the collider is required. I'd really like to just drop my script onto the root of my object and have it all "just work".

To think about:
I don't think the portals will work well for split-screen multiplayer; it might be possible to put the portals in the player template and use per-player camera culling as a hack. Per-player/maincamera rendertextures would probably be better.
I think I need some function to detect if screen resolution changes, as the rendertesxtures will need resizing.
