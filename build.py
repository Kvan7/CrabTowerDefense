import argparse
import os
import re
import subprocess
import sys
from time import time

class colors:
    '''Terminal colors'''
    HEADER = '\033[95m'
    OKBLUE = '\033[94m'
    OKCYAN = '\033[96m'
    OKGREEN = '\033[92m'
    WARNING = '\033[93m'
    FAIL = '\033[91m'
    ENDC = '\033[0m'
    BOLD = '\033[1m'
    UNDERLINE = '\033[4m'


def dir_path(string):
    '''Path validation for Unity directory'''
    if os.path.isdir(string) and os.path.isfile(os.path.join(string, 'Unity.exe')):
        return string
    else:
        raise FileNotFoundError(colors.FAIL + f"'Unity.exe' not found in the specified directory: {string}" + colors.ENDC)


def main():
    default_unity_path = "C:\\Program Files\\Unity\\Hub\\Editor\\2022.3.17f1\\Editor"

    parser = argparse.ArgumentParser(prog='Build', description='Unity Project Build Script')
    parser.add_argument('-p', '--platform', choices=['android', 'linux', 'windows'], help='OS platform target (default: builds all platforms)')
    parser.add_argument('-d', '--deployment_type', choices=['client', 'server'], help='type of deployment (default: builds all platforms)')
    parser.add_argument('-u', '--unity_path', type=dir_path, default=default_unity_path, help=f'path to unity (default: {default_unity_path})')

    try:
        args = parser.parse_args(sys.argv[1:])
    except Exception as e:
        # Argument Parsing Errors
        print(colors.FAIL + f"An exception of type {type(e).__name__} occurred. Arguments: {e.args}" + colors.ENDC)
        return

    platform = args.platform
    deployment_type = args.deployment_type
    unity_path = os.path.join(args.unity_path, 'Unity.exe')

    if platform == 'android' and deployment_type == 'client':
        method = "BuildScript.BuildAndroidClient"
    elif platform == 'linux' and deployment_type == 'server':
        method = "BuildScript.BuildLinuxServer"
    elif platform == 'windows' and deployment_type == 'client':
        method = "BuildScript.BuildWindowsClient"
    elif platform == 'windows' and deployment_type == 'server':
        method = "BuildScript.BuildWindowsServer"
    else:
        method = "BuildScript.BuildAll"

    try:
        print(colors.OKBLUE + 'Starting Build...' + colors.ENDC)
        t1 = time()
        result = subprocess.run([unity_path, "-quit", "-batchmode", "-projectPath", os.getcwd(), "-executeMethod", method, "-logfile", "-"], check=True, shell=True, capture_output=True)
        duration = time() - t1

        result.check_returncode()
        print(colors.OKBLUE + f'Build completed in {duration:.4f} seconds.' + colors.ENDC)

        if result.stdout != None:
            # Checking output for Unity Build Errors
            logfile = result.stdout.decode('utf-8')
            pattern = r"Build Failed(?:\r\n|\n\n)(.*?)(?=\r\n|\n\n)"
            matches = re.findall(pattern, logfile, re.DOTALL)

            if len(matches) > 0:
                print(colors.FAIL + 'Build Failed!' + colors.ENDC)
                for match in matches:
                    print(colors.FAIL + match + colors.ENDC)
            else:
                print(colors.OKGREEN + 'Build Success!' + colors.ENDC)
        else:
            print(colors.WARNING + 'No output from Unity process, potential failed build.' + colors.ENDC)
    except Exception as e:
        # Python Process Errors
        print(colors.FAIL + 'Build Failed!' + colors.ENDC)
        print(colors.FAIL + str(e) + colors.ENDC)


if __name__ == "__main__":
    main()