<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import { useShoppingCartStore } from '@/stores/shoppingCartStore';
import ShoppingCartDrawer from './ShoppingCartDrawer.vue';

const shoppingCartStore = useShoppingCartStore();
const fullClassName = computed(() => ({
    'shopping-cart': true,
    'shopping-cart_filled': !shoppingCartStore.isEmpty
}))

const isDrawerOpen = ref(false);
watch(isDrawerOpen, () => {
    document.body.classList.toggle('js-scroll-lock');
})
</script>

<template>
    <button :class="fullClassName"
            @click="isDrawerOpen = true">
        <span class="shopping-cart__text">
            Корзина
        </span>
        <template v-if="!shoppingCartStore.isEmpty">
            <span class="shopping-cart__delimiter">
                |
            </span>
            <span class="shopping-cart__product-quantity">
                {{ shoppingCartStore.totalQuantity }}
            </span>
        </template>
    </button>

    <Teleport to="body">
        <Transition name="drawer">
            <ShoppingCartDrawer v-if="isDrawerOpen" @close="isDrawerOpen = false" />
        </Transition>
    </Teleport>

</template>

<style lang="less" scoped>
.shopping-cart {
    font-size: inherit;
    border: 1px solid var(--brand-color);
    border-radius: var(--border-radius-item);
    padding: 0.5rem 1rem;
    min-width: 5rem;
    cursor: pointer;
    transition: background-color 200ms ease-out;
    background: none;

    &:hover {
        background-color: var(--brand-color);
    }

    &_filled {
        background-color: var(--brand-color);
    }

    &_filled:hover {
        background-color: var(--brand-color-darken);
    }
}

.drawer-enter-active,
.drawer-leave-active {
  transition: transform 0.2s ease-out;
}

.drawer-enter-from,
.drawer-leave-to {
    transform: translateX(100%);
}
</style>