import Cookies from 'js-cookie'

const TokenKey = 'assess_ms_token'

export function getCookies(key = TokenKey) {
  return Cookies.get(key)
}

export function setCookies(value, remember = false, key = TokenKey) {
  if (remember) {
    return Cookies.set(key, value, { expires: 365 });
  } else {
    return Cookies.set(key, value);
  }
}

export function removeCookies(key = TokenKey) {
  return Cookies.remove(key)
}