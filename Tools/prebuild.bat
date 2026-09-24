rem this needs to be run using a start /wait command or else the web service will fail. 
rem To Run the translate server: 
rem			py -m libretranslate.main --port 5000 
rem
cd
cd %~p0
cd 
Lyt.Translator.Cli.exe PaletteDesignerLanguages.json
rem pause
cd 
exit 0
