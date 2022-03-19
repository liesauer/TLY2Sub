<?php

function pad_zero($data, $length) {
    $dataLength = strlen($data);

    $padNeed = $dataLength % $length;

    if ($padNeed == 0) return $data;

    $padLength = $length - $padNeed;

    $data .= str_repeat("\0", $padLength);

    return $data;
}

function aes_encrypt($content, $key, $iv) {
    $method = 'aes-128-cbc';
    $ivLength = openssl_cipher_iv_length($method);

    $padContent = pad_zero($content, $ivLength);

    $encrypted = openssl_encrypt($padContent, $method, $key, OPENSSL_RAW_DATA | OPENSSL_ZERO_PADDING, $iv);

    if ($encrypted === FALSE) return "";

    return base64_encode($encrypted);
}

function aes_decrypt($content, $key, $iv) {
    $content = base64_decode($content);

    $method = 'aes-128-cbc';
    $ivLength = openssl_cipher_iv_length($method);

    $padContent = pad_zero($content, $ivLength);

    $decrypted = openssl_decrypt($padContent, $method, $key, OPENSSL_RAW_DATA | OPENSSL_ZERO_PADDING, $iv);

    if ($decrypted === FALSE) return "";

    return trim($decrypted, "\0");
}
