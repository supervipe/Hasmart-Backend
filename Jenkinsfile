pipeline {
  agent any
  stages {
    stage('build') {
      steps {
        echo 'Building'
        sh 'docker-compose down'
        sh 'docker-compose build'
      }
    }

    stage('deploy') {
      steps {
        echo 'Deploying'
        sh 'TOKEN_URL=http://140.238.182.243:5050 docker-compose up -d'
      }
    }

  }
}