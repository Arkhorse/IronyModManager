@echo off
cd ..
for /d %%i in (IronyModManager*) do (
	RD /s /q "%%i\bin"
)