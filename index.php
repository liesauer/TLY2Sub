<?php

$config = require_once __DIR__ . '/src/config.php';

if (!isset($_GET['pass']) || $_GET['pass'] != $config['pass']) exit();

require_once __DIR__ . '/src/tly2sub.php';
