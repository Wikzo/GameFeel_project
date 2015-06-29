<?php 
        $db = mysql_connect('HOST', 'LOGIN', 'PASSWORD') or die('Could not connect: ' . mysql_error()); 
        mysql_select_db('DATABASE') or die('Could not select database');
 
        // Strings must be escaped to prevent SQL injection attack. 
        $GUID = mysql_real_escape_string($_GET['GUID'], $db); 
        $Date = mysql_real_escape_string($_GET['Date'], $db); 
        $hash = $_GET['hash']; 
 
        $secretKey="KEY"; # Change this value to match the value stored in the client javascript below 

        $real_hash = md5($GUID . $secretKey); 
        if($real_hash == $hash) { 
            // Send variables for the MySQL database class. 
            $query = "insert into LatinSquaresData values (
                NULL,
                '$GUID',
                '$Date');"; 
            
            $result = mysql_query($query) or die('Query failed: ' . mysql_error()); 
        
            echo $query;
            echo $result;
        } 
        else
            echo "hash not correct";
?>