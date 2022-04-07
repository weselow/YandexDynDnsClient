![This is an image](/Sicons-Basic-Round-Social-Yandex.ico)
# Yandex Dynamic Dns Client

Легкий клиент для автоматического обновления динамического ip-адреса домена или поддомена в [Яндекс.Коннект](https://connect.yandex.ru).


## Логика работы

При первом запуске программа создает файл настроек `settings.json` и спрашивает настроки - домен, поддомен и токен доступа к Яндекс API.

Основной домен должен быть уже добавлен в [Яндекс.Коннект](https://connect.yandex.ru).

Програма создаст запись с поддоменом и сохранит текущий ip.

При последующем запуске проверит, не изменился ли ip, и если изменился - обновит его. Такая проверка встроена, чтобы лишний раз яндекс не дергать запросами.

Токен получить можно тут: https://pddimp.yandex.ru/api2/admin/get_token

Клиент рассчитан на работу через cron или расписание, в случае отсутствия ошибок он должен запускаться, отрабатывать и закрываться без уведомлений пользователя.


## Releases
Скачать скомплированную версию можно в разделе Releases: https://github.com/weselow/YandexDynDnsClient/releases


## Ссылки

Яндекс.Коннект: https://connect.yandex.ru

Яндекс API: https://yandex.ru/dev/pdd/doc/reference/dns-add.html

Получить токен: https://pddimp.yandex.ru/api2/admin/get_token
