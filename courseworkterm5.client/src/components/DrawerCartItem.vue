<script setup lang="ts">
import type { Purchase } from '@/stores/shoppingCartStore';
import { computed } from 'vue';
import { useShoppingCartStore } from '@/stores/shoppingCartStore';

const shoppingCartStore = useShoppingCartStore();

const props = defineProps<Purchase>();

const fullPriceRub = computed(() => {
    return props.quantity * props.cartItem.priceRub;
});

const productName = computed(() => {
    if (props.cartItem.diameterCm) {
        return `${props.cartItem.name}, ${props.cartItem.diameterCm} см`
    }

    return `${props.cartItem.name}`;
});

const isIncrementDisabled = computed(() => {
    return props.quantity === 9;
});
</script>

<template>
    <div class="cart-item">
        <div class="cart-item__info">
            <header class="cart-item__header">
                <img class="cart-item__image"
                     :src="cartItem.imageUrl.toString()">
                <svg class="cart-item__remove-icon"
                     @click="shoppingCartStore.removePurchase($props)"
                     xmlns="http://www.w3.org/2000/svg" xml:space="preserve" width="14"
                     height="14" version="1.1" viewBox="0 0 14 14" fill="#3a3a3a">
                    <path
                          d="M.3 13.7c.2.2.4.3.7.3.3 0 .5-.1.7-.3L7 8.4l5.3 5.3c.2.2.5.3.7.3.2 0 .5-.1.7-.3.4-.4.4-1 0-1.4L8.4 7l5.3-5.3c.4-.4.4-1 0-1.4-.4-.4-1-.4-1.4 0L7 5.6 1.7.3C1.3-.1.7-.1.3.3c-.4.4-.4 1 0 1.4L5.6 7 .3 12.3c-.4.4-.4 1 0 1.4z" />
                </svg>
            </header>

            <main class="cart-item__main">
                <h2 class="cart-item__name">
                    {{ productName }}
                </h2>
                <p class="cart-item__description" v-if="cartItem.description">
                    {{ cartItem.description }}
                </p>
                <ul class="cart-item__toppings" v-if="cartItem.toppings">
                    <li class="cart-item__topping" v-for="topping of cartItem.toppings">
                        {{ topping.name }},&nbsp;{{ `${topping.weightGram} г` }}
                    </li>
                </ul>
            </main>
        </div>

        <footer class="cart-item__footer">
            <div class="cart-item__full-price">
                <span class="cart-item__price-value">
                    {{ fullPriceRub }}
                </span>
                <span class="cart-item__currency">
                    &#x20bd;
                </span>
                
            </div>

            <div class="quantity-selector">
                <button class="button button_type_decrement"
                        @click="shoppingCartStore.removeOneProductUnit(cartItem)"
                        @click.native.prevent>
                    −
                </button>
                <input class="quantity-selector__input" type="number" :value="quantity" readonly>
                <button class="button button_type_increment"
                        @click="shoppingCartStore.addOneProductUnit(cartItem)" @click.native.prevent
                        :disabled="isIncrementDisabled">
                    +
                </button>
            </div>
        </footer>
    </div>

</template>

<style lang="less" scoped>
.cart-item {
    position: relative;

    &__info {
        display: grid;
        grid-template-columns: 1fr 3fr;
        gap: 0.5rem;
    }

    &__remove-icon {
        position: absolute;
        top: 0rem;
        right: 0rem;

        cursor: pointer;
        width: 1.75rem;
        height: 1.75rem;
        padding: 0.5rem;

        &:hover {
            fill: var(--brand-black);
        }
    }

    &__image {
        width: 100%;
        height: auto;
    }

    &__name {
        font-size: var(--text-lg);
    }

    &__description {
        font-size: var(--text-sm);
    }

    &__toppings {
        list-style-type: '+ ';
        list-style-position: inside;
    }

    &__topping {
        font-size: var(--text-sm);
    }

    &__footer {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    &__price-value {
        font-weight: 500;
    }
}

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button {
    -webkit-appearance: none;
    margin: 0;
}

input[type="number"] {
    -moz-appearance: textfield;
}

.quantity-selector {
    display: flex;
    font-size: var(--text-sm);

    &__input {
        font-size: inherit;
        width: 2rem;
        text-align: center;
        border: none;
        background-color: var(--color-background-soft);
    }
}

.button {
    font-size: inherit;
    padding: 0.25rem;
    transition: background-color 200ms ease-out;
    background-color: var(--brand-color);
    border: none;
    cursor: pointer;
    z-index: 2;

    &:disabled {
        background-color: var(--brand-disabled);
    }

    &_type {
        &_decrement {
            border-top-left-radius: var(--border-radius-item);
            border-bottom-left-radius: var(--border-radius-item);
        }

        &_increment {
            border-top-right-radius: var(--border-radius-item);
            border-bottom-right-radius: var(--border-radius-item);
        }
    }

    &:hover {
        background-color: var(--brand-color-darken);
    }

    &:active {
        outline: 1px solid var(--brand-black);
    }
}
</style>