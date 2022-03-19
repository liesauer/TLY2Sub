<?php

function get_vmess_link($vmess) {
    $data = [
        'v'    => '2',
        'ps'   => $vmess['remark'],
        'add'  => $vmess['host_name'],
        'port' => $vmess['port'],
        'id'   => $vmess['user_id'],
        'aid'  => $vmess['alter_id'],
        'net'  => $vmess['protocol'],
        'type' => $vmess['type'],
        'host' => $vmess['host'],
        'path' => $vmess['ws_path'],
        'tls'  => $vmess['tls'],
    ];

    return 'vmess://' . urlsafe_base64_encode(json_encode($data));
}

function urlsafe_base64_encode($content) {
    return str_replace(['+', '/', '='], ['-', '_', ''], base64_encode($content));
}

function urlsafe_base64_decode($content) {
    $content = str_replace(['-', '_'], ['+', '/'], $content);
    $len = strlen($content);

    if ($len % 4 != 0) {
        $content .= str_repeat('=', $len % 4);
    }

    return base64_decode($content);
}
