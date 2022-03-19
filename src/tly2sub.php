<?php

require_once __DIR__ . '/../vendor/autoload.php';
require_once __DIR__ . '/aes.php';
require_once __DIR__ . '/util.php';
$config = require_once __DIR__ . '/config.php';

$email  = $config['email'];
$passwd = $config['passwd'];
$key    = $config['aes_key'];
$iv     = $config['aes_iv'];

use liesauer\SimpleHttpClient;

$api = 'https://win.tly08.com/api/E-win2.php';

$response = SimpleHttpClient::quickPost($api, [
    'Content-Type' => 'application/x-www-form-urlencoded',
], null, [
    'a' => aes_encrypt($email, $key, $iv),
    'b' => $passwd,
]);

$data = $response['data'];

$json = json_decode(aes_decrypt($data, $key, $iv), TRUE);

if (empty($json) || $json['ret'] != 'ok') echo '';

$node_list = [];

foreach ($json['node'] as $node) {
    if ($node['node_method'] != 'ws') continue;

    array_push($node_list, [
        'remark'     => $node['node_name'],
        'host_name'  => $node['node_server'],
        'port'       => $node['port'],
        'user_id'    => $node['pass'],
        'alter_id'   => (int)$node['alterId'],
        'protocol'   => 'ws',
        'type'       => 'none',
        'host'       => '',
        'ws_path'    => $node['ws-path'],
        'tls'        => 'tls',
    ]);
}

array_push($node_list, [
    'remark'     => "剩余流量：${json['transfer']}G",
    'host_name'  => '127.0.0.1',
    'port'       => 8080,
    'user_id'    => '',
    'alter_id'   => 0,
    'protocol'   => 'ws',
    'type'       => 'none',
    'host'       => '',
    'ws_path'    => '',
    'tls'        => 'tls',
]);

$node_list = array_map(function ($node) {
    return get_vmess_link($node);
}, $node_list);

echo urlsafe_base64_encode(implode("\n", $node_list));
