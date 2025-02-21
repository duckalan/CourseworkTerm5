<script setup lang="ts">
import { onMounted } from 'vue';
import { useShoppingCartStore } from '@/stores/shoppingCartStore';
import DrawerCartItem from './DrawerCartItem.vue';
import SiteButton from './SiteButton.vue';
import { ButtonSize, ButtonType } from '@/enums/ButtonEnums';

const shoppingCartStore = useShoppingCartStore();
const emit = defineEmits(['close']);

onMounted(() => {
    const drawerMask = document.querySelector('.drawer-mask');
    drawerMask?.addEventListener('click', (event) => {
        if (event.target === event.currentTarget) {
            emit('close');
        }
    })
})
</script>

<template>
    <div class="drawer-mask">
        <section class="shopping-cart-drawer">
            <h1 class="shopping-cart-drawer__title">Корзина</h1>
            <svg class="shopping-cart-drawer__close-icon" @click="$emit('close')"
                 xmlns="http://www.w3.org/2000/svg" xml:space="preserve" width="14"
                 height="14" version="1.1" viewBox="0 0 14 14" fill="#3a3a3a">
                <path
                      d="M.3 13.7c.2.2.4.3.7.3.3 0 .5-.1.7-.3L7 8.4l5.3 5.3c.2.2.5.3.7.3.2 0 .5-.1.7-.3.4-.4.4-1 0-1.4L8.4 7l5.3-5.3c.4-.4.4-1 0-1.4-.4-.4-1-.4-1.4 0L7 5.6 1.7.3C1.3-.1.7-.1.3.3c-.4.4-.4 1 0 1.4L5.6 7 .3 12.3c-.4.4-.4 1 0 1.4z" />
            </svg>

            <h3 class="shopping-cart-drawer__fallback"
                v-if="shoppingCartStore.isEmpty">
                Пока что здесь пусто. Вы можете добавить сюда продукты.
            </h3>

            <template v-else>
                <ul class="shopping-cart-drawer__products">
                <li class="shopping-cart-drawer__product"
                    v-for="item of shoppingCartStore.items"
                    :key="item.cartItem.productId">
                    <DrawerCartItem v-bind="item" />
                </li>
            </ul>

            <footer class="shopping-cart-drawer__footer">
                <div class="shopping-cart-drawer__summarize">
                    {{ shoppingCartStore.totalQuantity }}
                    продуктов на
                    {{ shoppingCartStore.totalPriceRub }} &#x20bd;
                </div>
                <RouterLink to="/order"
                            class="link"
                            @click="$emit('close')">
                    Перейти к оформлению заказа
                </RouterLink>
            </footer>
            </template>
            
        </section>
    </div>
</template>

<style lang="less" scoped>
.drawer-mask {
    min-width: 100vw;
    min-height: 100vh;
    z-index: 9999;
    background-color: rgb(0 0 0 / 0.5);
    position: fixed;
    top: 0;
    left: 0;
    cursor: pointer;
}

.shopping-cart-drawer {
    --drawer-padding: 1rem;

    background-color: var(--color-background-soft);
    height: 100vh;
    width: 30vw;

    position: absolute;
    top: 0;
    right: 0;
    cursor: auto;

    &__title {
        padding: var(--drawer-padding);
        font-size: var(--text-xxl);
    }

    &__fallback {
        padding-inline: var(--drawer-padding);
    }

    &__close-icon {
        position: absolute;
        top: calc(var(--drawer-padding));
        right: calc(var(--drawer-padding));
        cursor: pointer;
        width: calc(var(--text-xxl) + 0.5rem);
        height: calc(var(--text-xxl) + 0.5rem);
        padding: 0.5rem;


        &:hover {
            fill: var(--brand-black);
        }
    }

    &__products {
        height: 70vh;
        overflow-y: auto;
        list-style: none;
        scrollbar-width: thin;
    }

    &__product {
        padding: var(--drawer-padding);
        background-color: var(--color-background);
        margin-bottom: 1rem;
    }

    &__footer {
        padding: var(--drawer-padding);
        height: 100%;
    }

    &__summarize {
        margin-bottom: 0.5rem;
    }
}

.link {
    display: flex;
    justify-content: center;
    align-items: center;
    border: none;
    border-radius: var(--border-radius-item);
    padding: 0.5rem 1rem;
    width: 100%;
    cursor: pointer;
    transition: background-color 200ms ease-out;
    background-color: var(--brand-color);
    color: var(--color-text);
    font-size: var(--text-md);
    text-decoration: none;

    &:hover {
        background-color: var(--brand-color-darken);
    }

    &:active {
        border: 1px solid var(--brand-black);
    }
}
</style>