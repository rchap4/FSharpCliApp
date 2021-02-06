pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
                dotnetRestore()
                dotnetBuild()
                
                archiveArtifacts artifacts: '**/bin/Debug/net5.0/*.dll', fingerprint: true, followSymlinks: false, onlyIfSuccessful: true
                archiveArtifacts artifacts: '**/bin/Debug/net5.0/*.exe', fingerprint: true, followSymlinks: false, onlyIfSuccessful: true
            }
        }
    }
}