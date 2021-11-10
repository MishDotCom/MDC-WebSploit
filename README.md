# MDC-WebSploit 
[![version-1.6](https://img.shields.io/badge/version-1.6-green)](https://github.com/MishDotCom/WebSploit/releases/tag/v1.6)
[![GPLv3](https://img.shields.io/badge/license-GPLv2-blue)](https://img.shields.io/badge/license-GPLv3-blue)
[![C#](https://img.shields.io/badge/language-csharp-red)](https://img.shields.io/badge/language-c#-red)

<h2>Description</h2>

**WebSploit** is a web stress testing and pentesting tool, offering a useful set of tools to perform **web level** attacks. This app is meant to be ran in a command line interface
with the approppiate arguments. This app is meant to combine hacking tools into one big menu to make hacking easier and more fun. MDC-WebSploit currently has tools for credential
bruteforcing, information gathering and stress testing.

Disclaimer: **I am under no circumstances responsible of any wrong way this app is used. Meant only for educational purposes and white hat hacking.**

<h2>Commands and Tools 🧰</h2>

MDC-WebSploit is supposed to be ran in a terminal or a CLI. The commands and arguments are >>

```text
---------------------------------------------------
       MDC-WebSploit v1.6 CLI. Help Menu >>        
---------------------------------------------------
>> Information gathering:
'-prtsc' - Multithreaded port scanner.
       [SYNTAX] : -prtsc <TARGET> <OPTION>
       > '-prtsc' Options:
          '-p' Scans only one given port.
          '-P' Scans a given list of comma sepraretd ports.
          '--p <OPTION>' Scans built-in list of ports.
                > '--p' Options:
                   '-20' Top 20 most common ports.
                   '-200' Top 200 most common ports.
'-nscn' - Network scanning tool.
          [SYNATX] : -nscn <OPTION> <TARGET>
          > '-nscn' Options :
            '-gh' Retreives host of given website.
            '-ph <OPTION>' Pings a given host.
             > '-ph' Options: [OPTIONAL]
               '-b <NUM Of BYTES>' Sets the ping number of bytes.
            '-pLan' Finds open hosts on the given network.
>> Passwords and credential attacks:
'-ftpl' - FTP Server credentials bruteforcing app.
          [SYNATX] : -ftpl <TARGET> <USR LIST PATH> <PASS LIST PATH>
          > '-ftpl' Options:
             '-b' For built-in dictionaries.
             ex: [SYNATX] : -ftpl <TARGET> -b -b
'-sshf' - SSH Server credentials bruteforcing app.
          [SYNATX] : -sshf <TARGET> <USR LIST PATH> <PASS LIST PATH>
          > '-sshf' Options:
             '-b' For built-in dictionaries.
             ex: [SYNATX] : -sshf <TARGET> -b -b
'-ecrk' - Email (SMTP) credentials bruteforce app.
          [SYNTAX] : -ecrk <SMTP_SERVER> <OPT: '-s' for SSL> <SMTP_PORT> <TARGET> <-P <PATH TO WORDLIST>/--p for built-in (rockyou.txt)> <OPT: -v for verbose>
'-webcrk' - Website login cracking tool.
          [SYNTAX] : -webcrk <LOGIN_URL> <FORM DATA (separated by comma. mark the password field with [p] and the username field with [u].) ex: [u]email,[p]password, action:Login> <TARGET (username/email)> <-p <ONE password> / -P <path to wordlist> / --p (built-in : rockyou.txt)> <OPT: -v (for verbose)>
>> Stress testing:
'-uddos' Universal complete DDoS attack tool.
         [SYNTAX] : -uddos <PROTOCOL> <PROTOCOL_TASK> <TARGET> <THREADS>
         > '-uddos' Protocols:
            '--http' Stress testing for HTTP websites.
            > '--http' Tasks:
                '-d' DDoS through multiple download requests.
                '-g' DDoS through multiple get requests.
                '-b' DDoS through multiple download and get requests. [SLOW]
            '--https' Stress testing for HTTPS websites.
            > '--https' Tasks:
                '-d' DDoS through multiple download requests.
                '-g' DDoS through multiple get requests.
                '-b' DDoS through multiple download and get requests. [SLOW]
            '--tcp' Stress testing for TCP servers.
            > '--tcp' Tasks:
                '-h' DDoS through packet flooding.
                '-e' DDoS through multiple connection requests.
                Target Format : <IP_Address>:<Port> ex: [127.0.0.1:80]
                [WARNING] : The target TCP port MUST be open or the requests will bounce back!
            '--udp' Stress testing for UDP servers.
            > '--udp' Tasks:
                '-h' DDoS through packet flooding.
                '-e' DDoS through multiple connection requests.
                Target Format : <IP_Address>:<Port> ex: [127.0.0.1:80]
                [WARNING] : The target UDP port MUST be open or the requests will bounce back!
---------------------------------------------------
```

## Installation ⚙️

<h2>**Use the installer:**</h2>
1. For Linux: https://github.com/MishDotCom/mdc-ws-installer/releases/download/v1.1/wsinstall32<br>
2. For Windows: https://github.com/MishDotCom/mdc-ws-installer/releases/download/v1.1/wsinstall32.exe

<h2>Manual Instalation:</h2>

<h2>For windows:</h2>
1. Download the folder called <code>websploit win-x86.zip</code>.<br>
2. Extract the folder and place it in a directory of your choice.<br>
3. Run the file called <code>WebSploit.exe</code> in CMD or PowerShell as Administartor.<br>

**OR** Download the windows installer from this link: https://github.com/MishDotCom/mdc-ws-installer/releases/download/v1.0/mdc-WebSploit.Installer.exe

Disclaimer: **Do NOT remove any of the .dll files or the app will cease to work.**<br>

<h2>For linux-debian:</h2>
1. Download the folder called <code>WebSploit deb-x64.zip</code><br>
2. Extract the folder.<br>
3. Open a terminal and CD into the folder.<br>
4. Run <code>chmod 777 ./WebSploit</code><br>
5. Run the app using <code>sudo ./WebSploit</code><br>
