# TriviaApi

# How to build
1. Install [NodeJs](https://nodejs.org/).
2. Install [Python](https://www.python.org/downloads/). Do not forget to check checkbox ```Add Python 3.xxx to PATH```.
2. Download UI part from [GoogleDrive](https://drive.google.com/drive/folders/1vtRdVSawPbA0FHuMNdkYWdluoxFUVvsS?usp=sharing)
3. Extract downloaded archive to some folder.
4. Open CMD and go to this folder using ```cd``` [command](https://en.wikipedia.org/wiki/Cd_(command)).
5. Run ```npm i``` and then ```npm run build ```.
6. Run ```python -m http.server```. By default it will serving on HTTP port 8000.
7. Download source files of current project. Run it.
8. Open new tab in browser and go to [http://localhost:8000/](http://localhost:8000/). Use [https://localhost:5001/](https://localhost:5001/) as input for dialog window.
9. Duplicate previously created tab and do the same work.
Game will start.
# Known bug (UI side)
1. Create game between two players
2. Choose any answers for question
3. Let one player press "Exit" and another one press "Play again"
4. User which selected "Play again" should give message "OpponentLeave".

Looks like there is no "Leave()" called on hub after "Exit" clicked.
