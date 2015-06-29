<?php 
        $db = mysql_connect('SERVER', 'DOMAIN', 'PASSWORD') or die('Could not connect: ' . mysql_error()); 
        mysql_select_db('DATABASE') or die('Could not select database');
 
        // Strings must be escaped to prevent SQL injection attack. 
        $GUID = mysql_real_escape_string($_GET['GUID'], $db); 
        $Platform = mysql_real_escape_string($_GET['Platform'], $db); 
        $Gender = mysql_real_escape_string($_GET['Gender'], $db); 
        $Age = mysql_real_escape_string($_GET['Age'], $db); 
        $Country = mysql_real_escape_string($_GET['Country'], $db); 
        $ExperienceGames = mysql_real_escape_string($_GET['ExperienceGames'], $db); 
        $ExperiencePlatformers = mysql_real_escape_string($_GET['ExperiencePlatformers'], $db); 
        $Date = mysql_real_escape_string($_GET['Date'], $db); 
        $GroundDescription = mysql_real_escape_string($_GET['GroundDescription'], $db); 
        $AirDescription = mysql_real_escape_string($_GET['AirDescription'], $db); 
        $Twitchy = mysql_real_escape_string($_GET['Twitchy'], $db); 
        $Fluid = mysql_real_escape_string($_GET['Fluid'], $db); 
        $Stiff = mysql_real_escape_string($_GET['Stiff'], $db); 
        $Floaty = mysql_real_escape_string($_GET['Floaty'], $db); 
        $Responsive = mysql_real_escape_string($_GET['Responsive'], $db); 
        $Enjoyable = mysql_real_escape_string($_GET['Enjoyable'], $db); 
        $Difficult = mysql_real_escape_string($_GET['Difficult'], $db); 
        $HowMuchLike = mysql_real_escape_string($_GET['HowMuchLike'], $db); 
        $HowFrustrated = mysql_real_escape_string($_GET['HowFrustrated'], $db);
        $Gravity = mysql_real_escape_string($_GET['Gravity'], $db); 
        $JumpPower = mysql_real_escape_string($_GET['JumpPower'], $db); 
        $AirFrictionHorizontalPercentage = mysql_real_escape_string($_GET['AirFrictionHorizontalPercentage'], $db); 
        $TerminalVelocity = mysql_real_escape_string($_GET['TerminalVelocity'], $db); 
        $GhostJumpTime = mysql_real_escape_string($_GET['GhostJumpTime'], $db); 
        $MinimumJumpHeight = mysql_real_escape_string($_GET['MinimumJumpHeight'], $db); 
        $ReleaseEarlyJumpVelocity = mysql_real_escape_string($_GET['ReleaseEarlyJumpVelocity'], $db); 
        $ApexGravityMultiplier = mysql_real_escape_string($_GET['ApexGravityMultiplier'], $db); 
        $MaxVelocityX = mysql_real_escape_string($_GET['MaxVelocityX'], $db); 
        $ReleaseTime = mysql_real_escape_string($_GET['ReleaseTime'], $db);
        $AttackTime = mysql_real_escape_string($_GET['AttackTime'], $db);
        $AnimationMaxSpeed = mysql_real_escape_string($_GET['AnimationMaxSpeed'], $db);
        $Level = mysql_real_escape_string($_GET['Level'], $db);
        $Deaths = mysql_real_escape_string($_GET['Deaths'], $db);
        $TimeSpentOnLevel = mysql_real_escape_string($_GET['TimeSpentOnLevel'], $db);
        $FPS = mysql_real_escape_string($_GET['FPS'], $db);
        $LatinSequence = mysql_real_escape_string($_GET['LatinSequence'], $db);
        $hash = $_GET['hash']; 
 
        $secretKey="KEY"; # Change this value to match the value stored in the client javascript below 

        $real_hash = md5($GUID . $secretKey); 
        if($real_hash == $hash) { 
            // Send variables for the MySQL database class. 
            //$query = "insert into feeling6 values (NULL, '$GUID', '$Gender', '$Age', '$Country', $ExperienceGames', '$ExperiencePlatformers', '$Date', '$GroundDescription', '$AirDescription', '$Twitchy', '$Fluid', '$Stiff', '$Floaty', '$Responsive', '$Enjoyable', '$Difficult', '$HowMuchLike', '$HowFrustrated', '$Gravity', '$JumpPower', '$AirFrictionHorizontalPercentage', '$TerminalVelocity', '$GhostJumpTime', '$MinimumJumpHeight', '$ReleaseEarlyJumpVelocity', '$ApexGravityMultiplier', '$MaxVelocityX', '$ReleaseTime', '$AttackTime', '$AnimationMaxSpeed', '$Level', '$Deaths', '$TimeSpentOnLevel', '$FPS', '$LatinSequence');"; 
            //$query = "insert into feeling6 values (NULL, '$GUID', '$Gender', '$LatinSequence');"; 
            $query = "insert into GameFeelData values (
                NULL,
                '$GUID',
                '$Platform',
                '$Gender',
                '$Age',
                '$Country',
                '$ExperienceGames',
                '$ExperiencePlatformers',
                '$Date',
                '$GroundDescription',
                '$AirDescription',
                '$Twitchy',
                '$Fluid',
                '$Stiff',
                '$Floaty',
                '$Responsive',
                '$Enjoyable',
                '$Difficult',
                '$HowMuchLike',
                '$HowFrustrated',
                '$Gravity',
                '$JumpPower',
                '$AirFrictionHorizontalPercentage',
                '$TerminalVelocity',
                '$GhostJumpTime',
                '$MinimumJumpHeight',
                '$ReleaseEarlyJumpVelocity',
                '$ApexGravityMultiplier',
                '$MaxVelocityX',
                '$ReleaseTime',
                '$AttackTime',
                '$AnimationMaxSpeed',
                '$Level',
                '$Deaths',
                '$TimeSpentOnLevel',
                '$FPS',
                '$LatinSequence');"; 
            
            $result = mysql_query($query) or die('Query failed: ' . mysql_error()); 
        
            echo $query;
            echo $result;
        } 
        else
            echo "hash not correct";
?>