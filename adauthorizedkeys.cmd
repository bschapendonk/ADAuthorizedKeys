@ECHO OFF
FOR /F "tokens=2 delims=\\" %%G in ("%1") DO (
	C:\Windows\System32\dsquery.exe * -filter "(&(objectClass=user)(sAMAccountName=%%G))" -attr sshPublicKeys -l
)
