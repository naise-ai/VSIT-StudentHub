const CACHE_NAME = "studenthub-v1";
const STATIC_ASSETS = [
  "/manifest.webmanifest",
  "/icons/icon-192.png",
  "/icons/icon-512.png",
  "/icons/icon.svg",
  "/css/site.css",
  "/js/site.js"
];

self.addEventListener("install", event => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => cache.addAll(STATIC_ASSETS))
      .then(() => self.skipWaiting())
  );
});

self.addEventListener("activate", event => {
  event.waitUntil(
    caches.keys()
      .then(keys => Promise.all(keys.filter(key => key !== CACHE_NAME).map(key => caches.delete(key))))
      .then(() => self.clients.claim())
  );
});

self.addEventListener("fetch", event => {
  const request = event.request;
  if (request.method !== "GET" || new URL(request.url).origin !== self.location.origin) return;

  const path = new URL(request.url).pathname;
  const isStaticAsset = ["/css/", "/js/", "/icons/", "/manifest.webmanifest"].some(prefix => path.startsWith(prefix));
  event.respondWith(
    isStaticAsset
      ? caches.match(request).then(cached => cached || fetch(request))
      : fetch(request).catch(() => caches.match(request))
  );
});
