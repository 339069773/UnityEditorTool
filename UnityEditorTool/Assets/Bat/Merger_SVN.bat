@echo off

@set svn_bin=C:\Program Files\TortoiseSVN\bin
@set svn_work=D:\WorkCopy\branch2
@set trunk_path=svn://192.168.0.50/dragonunity/trunk
@set mege_range=18556-18560,18562-18565,18567-18569,18571-18580,18583,18585-18587,18589-18596,18598-18599,18604-18606,18608-18610,18612-18616,18618-18628,18633-18645,18647-18671,18674-18678,18680,18682-18683,18685,18690
@set svn_version_split="-"
@set svn_log=Merged revision(s) from trunk : %mege_range%
:: ��ӡ
@echo   ������Ϣ
@echo ************************************************************************
@echo  URL to merge from ��%trunk_path%
@echo  Working Copy      ��%svn_work%
@echo ------------------------------------------------------------------------
@echo  specific range    ��%mege_range%
@echo ************************************************************************


 rem �жϿ�ִ���ļ�����Ŀ�ļ�Ŀ¼�Ƿ���ȷ
@if not exist "%svn_bin%\TortoiseProc.exe" (
    echo.
    echo error: ��ȷ��TortoiseSVN�ͻ���Ŀ¼��ȷ?
    echo Ŀǰ�������:%svn_bin% 
    echo.
    pause & exit 1
    )

@if not exist "%svn_work%\.svn" (
    echo.
    echo error: ��ȷ��SVN��Ŀ�ļ�Ŀ¼��ȷ?
    echo Ŀǰ�������:%svn_work% 
    echo.
    pause & exit 2
    )

if defined mege_range (
    echo --------start--------
) else (
	echo �ϲ�����Ϊ�� ��
	pause & exit 2
)

::echo SVNĿ¼����
echo ����svn...
"%svn_bin%"\TortoiseProc.exe/command:update /path:"%svn_work%" /closeonend:2
echo ������� !

rem �����汾
for %%a in (%mege_range%) do (
	echo;%%a|find %svn_version_split% >nul &&(
	rem ���ڡ�-�� ��������ϲ�
		for /f "delims=-, tokens=1,2" %%i in ("%%a") do (
			for /l %%b in (%%i,1,%%j) do (
				rem echo %%b
				call:TestMerge %%b
				call:CheckTestMergeResult %%b
			)
		)
	)||(
		rem �����汾
		rem echo %%a *
		call:TestMerge %%a
		call:CheckTestMergeResult %%a
	)
)
echo. ==================== Success!! ========
echo. ==================== Success!! ========
echo. ==================== Success!! ========
if exist MergeInfo.txt del MergeInfo.txt

echo %svn_log% | clip

:END
pause
GOTO:EOF

:TestMerge
echo. ==================== test merge %1 ... ========
svn merge %trunk_path% -c %1 --dry-run > MergeInfo.txt
GOTO:EOF

:CheckTestMergeResult
findstr .* MergeInfo.txt >nul
if errorlevel 1 (
	echo    result = false  reason : Is Empty Version OR Already Merge
) else (
	findstr "Tree conflicts" MergeInfo.txt >nul
	if errorlevel 1 (
		::�޳�ͻ ����merger
		echo    result = true  do merger...
		call:DoMerge %%1
	) else (
		echo    result = false  reason : Tree conflicts!!
		GOTO END
	)
)
GOTO:EOF

:DoMerge  
echo. ==================== do merge %1 ... ========
svn merge %trunk_path% -c %1
GOTO:EOF

