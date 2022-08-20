#wait for the SQL Server to come up
sleep 50s &&
echo 'Inicializando BD depois de 50s de espera'
#run the setup script to create the DB and the schema in the DB
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $MSSQL_SA_PASSWORD -d master -i /data/carga_inicial.sql