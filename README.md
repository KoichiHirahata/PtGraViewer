Patient Graphic File Viewer
====================

## INITIAL SETTINGS

1. Click "Settings".
1. Click "Browse" to select the folder containing the .jpg and .pdf files you previously saved. You may specify a shared folder or NAS.
1. Click "Save".

## USAGE

1. Fill in "Patient ID," then click "View" (or press Enter).
1. Your .jpg and .pdf files will appear on the left side of the window.
1. When you double-click a file (or select it and then press Enter), the series files will appear on the right side of the window.

## Command-line options

/pt: Patient's ID
/date: Date string.(YYYYMMDD)
e.g. PtGraViwer.exe /pt:1000 /date:20150131

## Software Limitations

If you use a drive formatted with FAT32, you may store 65,534 patient's files.  
We recommend using a drive formatted with NTFS.  
You may store 999 series/collections per patient per day.

## Licence

Licensed under the GPL v3

## Auther

[Koichi Hirahata](https://github.com/KoichiHirahata)
