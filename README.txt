    

    Instructions to reproduce data

    1. Download both the SeriousGame (BUILD) and ProcessSeriousGameData (BUILD).
    2. Playing the game (In SeriousGame (BUILD) > SeriousGameVersion1.exe) will generate data in the form of dat files.
    3. Copy the dat files from "C:\user\{username}\AppData\LocalLow\SeriousGameUniversityUnityProject\SeriousGameVersion1" the most imporant ones used in the results section are: responses.dat and sessiontimes.dat.
    4. If you can't find an AppData folder, you can press the windows key and search "%AppData%" and it will pop up in the windows file explorer.
    5. Run the ProcessSeriousGameData application (In ProcessSeriousGameData (BUILD) > ProcessSeriousGameData.exe) so it creates the files in AppData, now close this window.
    6. Paste the dat files in "C:\user\{username}\AppData\LocalLow\SeriousGameUniversityUnityProject\ProcessSeriousGameData".
    7. Once the files are copied and pasted there, run the ProcessSeriousGameData application again, now the dat files will have been processed into .csv files which will be in the same location you put the dat files.
    8. The participant data that was captured during their play is inside the ParticipantData folder and contains their respective dat files produced following these instructions.