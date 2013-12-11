import os
import subprocess
import sys
import time
import threading

monitoredProcesses = ["calc.exe", "werfault.exe"]

def MonitorProcesses(delay):
    global monitorRunning
    while monitorRunning:
        time.sleep(delay)
        tasks = os.popen("tasklist").read()
        for process in monitoredProcesses:
            if process.lower() in tasks.lower():
                print "Error: monitored process \"" + process + "\" encountered."
                monitorRunning = False
                KillTask(process)
                KillCreatedProcesses()              

def KillTask(task):
    subprocess.Popen("taskkill /F /IM " + task).communicate()

def KillTaskIfPresent(task):
    tasks = os.popen("tasklist").read()
    if task.lower() in tasks.lower():
        KillTask(task)

def KillCreatedProcesses():
    for process in createdProcesses:
        process.kill()

if __name__ == "__main__":
    monitorRunning = True
    createdProcesses = []  

    # Start monitor thread
    try:
        monitorThread = threading.Thread(target=MonitorProcesses, args=[2])
        monitorThread.start()
    except:
        print "Error: unable to start thread"
        sys.exit(1)    

    KillTaskIfPresent("notepad.exe")
    createdProcesses.append(subprocess.Popen("notepad.exe"))

    mainProcess = subprocess.Popen("rca.exe")
    createdProcesses.append(mainProcess)
    mainProcess.communicate() 

    KillCreatedProcesses()

    if monitorRunning:
        monitorRunning = False
        monitorThread.join()
        sys.exit(mainProcess.returncode)
    else:
        sys.exit(1)