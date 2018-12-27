makecert -pe -$ commercial -n CN=localhost test.cer -sv test.pvk -r -sky export
cert2spc test.cer test.spc
pvk2pfx -pvk test.pvk -pi Qwerty1 -spc test.spc