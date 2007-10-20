ReadMe.txt for BMFontGen.exe

This directory contains the sample files that were used to create
the fonts shown in the documentation.

Since the created fonts are white text on a transparent background,
you may have difficulty seeing the font if you try to view the font
bitmap. To make the font easier to view, you can add the -blackbg
option (this will create the font image in a black background) but
be sure to remove this option before creating your final font for
use in a game.


Sample 1:
Create 16 pt Times New Roman with support for basic latin characters.
> bmfontgen -optfile times-options.txt
Output files: times.xml, times-0.png, times-1.png


Sample 2:
Create 14 pt Comic Sans MS font with the 'q' character replaced by
a custom glyph extracted from abxy.png.
> bmfontgen -optfile comic-options.txt
Output files: comic.xml, comic-0.png


Sample 3:
Create 14 pt Russian font from Lucida Sans Unicode
> bmfontgen -optfile russian-options.txt
Output files: russian.xml, russian-0.png, russian-1.png


Sample 4:
Create 14 pt Japanese font from MS P Gothic
>bmfontgen -optfile japanese-options.txt
Output files: japanese.xml, japanese-0.png, japanese-1.png

