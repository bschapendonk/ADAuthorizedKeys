@echo off
C:\Windows\System32\dsquery.exe * -filter "(&(objectClass=user)(sAMAccountName=%1))" -attr sshPublicKeys -l
