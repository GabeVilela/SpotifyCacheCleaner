# SpotifyCacheCleaner

Usually, when you stream any song in Spotify client (either the Win32 or UWP versions), it stores some cache hidden within its own folder, which can be a problem if your C drive is getting full.

So, I decided to make a simple C# app that cleans this cache.

You can use Windows' Task scheduler to execute the app automatically, just be sure to NOT HAVE Spotify opened at the same time.

Also, it shouldn't delete the songs you've downloaded for offline mode, but either way, you can change where Spotify stores those (Preferences > Show advanced > Offline storage location).

***

## How to install it?

Just download the zip file from the lastest release, unzip and run the exe.

## How it configure it?

After you've opened it for the first time, it should automatically prompt you for the necessary info, which as jan 23th, 2022, is:
- The Spotify's ROOT folder directory (example: C:\Users\<Your username>\AppData\Local\Packages\SpotifyAB.SpotifyMusic_zpdnekdrzrea0)
- If you Spotify's is the Microsoft Store version
