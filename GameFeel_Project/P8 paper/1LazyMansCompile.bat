
:: Closing Adobe reader.
tasklist /FI "IMAGENAME eq AcroRd32.exe" | find /i "AcroRd32.exe" 
 
IF ERRORLEVEL 2 GOTO EXIST
IF ERRORLEVEL 1 GOTO NOTEXIST 
 
:EXIST
taskkill /F /IM "AcroRd32.exe"
set reopenvar=yes
goto FIRSTCLEANING
 
:NOTEXIST
set reopenvar=no
goto FIRSTCLEANING


:FIRSTCLEANING
IF NOT EXIST master.aux goto COMPILING
::Cleaning if last compile failed
del master.aux
del master.log
del master.out
del master.synctex.gz
del master.toc
del master.bbl
del master.synctex.gz(busy)
del master.blg
del /s *.aux
GOTO COMPILING

::ERROR
:ERRORMASTER
echo master.tex not found.
goto ENDWITHPAUSE

::Compiling
:COMPILING
IF NOT EXIST master.tex goto ERRORMASTER
pdflatex.exe master.tex
pdflatex.exe master.tex
bibtex.exe master.aux
pdflatex.exe master.tex
pdflatex.exe master.tex
del master.aux
del master.log
del master.out
del master.synctex.gz
del master.toc
del master.bbl
del master.synctex.gz(busy)
del master.blg
del /s *.aux

::Starting adobe reader again
:START
IF %reopenvar%==yes start acrord32.exe master.pdf
goto END

:ENDWITHPAUSE
pause

:END