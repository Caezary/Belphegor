# Belphegor

## Description

Belphegor is a small tool running in system tray (Windows only). Currently, its primary purpose is preventing early screen locking.

## How to build

Run `dotnet build Belphegor.sln`.
The created application can be found in the `bin` folder.

## How to use

Belphegor is a tray-only application. Use the tray icon to turn its singular feature on or off (it's turned off by default).

The busy idle feature has two modes of operation, configured in the appsettings.json file - SendKeys and ExecutionState.

**SendKeys** - in this mode, the idle time is checked periodically via a system call. Upon exceeding a certain threshold, a virtual keystroke is sent (currently the '+' sign is used for this). This method's drawback is that a foreground application with elevated privileges (i.e. ran as Administrator) will prevent a successful idle checking, thus blocking the whole routine.

**ExecutionState** - this method changes the application's execution state to "display required" via a system call.

## Improvements

Many things could be improved, for instance:
 * add a real DI
 * possibility of enabling both busy idle options at once
 * some constants could be parametrized
 * pluginability to make the tool more versatile

## External sources

<div>Icons made by <a href="https://www.freepik.com" title="Freepik">Freepik</a> from <a href="https://www.flaticon.com/" title="Flaticon">www.flaticon.com</a></div>