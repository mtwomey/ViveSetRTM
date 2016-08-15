# ViveSetRTM
Set the render target multiplier for SteamVR from the command line (Vive Supersampling).

```
setrtm [TARGET NUMBER] [/NR]
```

By default - if SteamVR is running, it will kill it, set the desired RTM, then restart it. If you specify */nr* it will only set the RTM and will not check or restart SteamVR.
