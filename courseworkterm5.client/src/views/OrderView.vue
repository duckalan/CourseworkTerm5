<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';

import SiteButton from '@/components/SiteButton.vue';
import { ButtonSize, ButtonType } from '@/enums/ButtonEnums';
import { useShoppingCartStore, resetShoppingCartState } from '@/stores/shoppingCartStore';
import type { OrderModel } from '@/types/Order';
import PaymentType from '@/types/PaymentType';
import DrawerCartItem from '@/components/DrawerCartItem.vue';

const shoppingCartStore = useShoppingCartStore();
const router = useRouter();
const order = ref<OrderModel>({
    customer: { fullName: '', email: '', phone: '' },
    paymentType: PaymentType.Online,
    totalPriceRub: shoppingCartStore.totalPriceRub,
    useShipping: true,
});

function sendOrder() {
    fetch('/api/createorder', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(order)
    }).then(response => console.log(response));

    resetShoppingCartState();
    router.push('/');
}
</script>

<template>
    <div class="container">
        <template v-if="shoppingCartStore.isEmpty">
            В корзине пока что пусто. Вы можете добавить сюда продукты.
        </template>

        <template v-else>
            <ul class="product-list">
                <li class="product-list__item"
                    v-for="cartItem of shoppingCartStore.items">
                    <DrawerCartItem
                                    :cart-item="cartItem.cartItem"
                                    :quantity="cartItem.quantity" />
                </li>
            </ul>

            <form class="order-form" @submit.prevent="sendOrder()">
                <div class="order-form__input">
                    <label class="order-form__label"
                           for="customer-name">
                        ФИО</label>
                    <input class="order-form__text-input"
                           type="text"
                           id="customer-name"
                           v-model="order.customer.fullName"
                           required>
                </div>

                <div class="order-form__input">
                    <label class="order-form__label"
                           for="customer-phone">
                        Телефон</label>
                    <input class="order-form__text-input"
                           type="tel"
                           id="customer-phone"
                           v-model="order.customer.phone"
                           required>
                </div>

                <div class="order-form__input">
                    <label class="order-form__label"
                           for="customer-email">
                        Электронная почта</label>
                    <input class="order-form__text-input"
                           type="email"
                           id="customer-email"
                           v-model="order.customer.email"
                           required>
                </div>

                <fieldset class="order-form__fieldset">
                    <div class="order-form__input">
                        <label class="order-form__label"
                               for="use-shipping">
                            Доставка на дом
                        </label>
                        <input class="order-form__radio"
                               type="radio"
                               id="use-shipping"
                               v-model="order.useShipping"
                               :value="true">
                    </div>

                    <div class="order-form__input">
                        <label class="order-form__label"
                               for="pickup">
                            Забрать самому
                        </label>
                        <input class="order-form__radio"
                               type="radio"
                               id="pickup"
                               v-model="order.useShipping"
                               :value="false">
                    </div>

                    <div class="order-form__input">
                        <label class="order-form__label"
                               for="customer-address">
                            Адрес для доставки</label>
                        <input class="order-form__text-input"
                               type="text"
                               id="customer-address"
                               v-model="order.customer.address"
                               :required="order.useShipping"
                               :disabled="!order.useShipping">
                    </div>
                </fieldset>

                <div class="order-form__input">
                    <select class="order-form__select" v-model="order.paymentType">
                        <option :value="PaymentType.Online" selected>Онлайн оплата</option>
                        <option :value="PaymentType.Cashless">Картой при получении</option>
                        <option :value="PaymentType.Cash">Наличными</option>
                    </select>
                </div>

                <div class="order-form__place-order">
                    Доставка: бесплатно
                    К оплате: {{ shoppingCartStore.totalPriceRub }}&nbsp;&#x20bd;

                    <SiteButton
                                :size="ButtonSize.Medium"
                                :type="ButtonType.Primary">
                        Оформить заказ
                    </SiteButton>
                </div>
            </form>
        </template>
    </div>
</template>

<style lang="less" scoped>
.product-list {
    background-color: var(--brand-pastel);
    padding: 1rem;
    margin: 1rem 0;
    list-style: none;
    border-radius: var(--border-radius-container);
    
    &__item {
        max-width: 75%;
        margin-bottom: 1rem;
    }
}

label:has(+ input[required])::after {
    content: "*";
    color: red;
}

input[type="radio"] {
    accent-color: var(--brand-color);
    width: 1rem;
    height: 1rem;
}

.order-form {
    font-size: var(--text-md);

    &__input {
        display: flex;
        flex-direction: column;

        margin-bottom: 0.5rem;
    }

    &__fieldset {
        margin-bottom: 0.5rem;
    }

    &__text-input {
        font-size: inherit;
        padding: 0.5rem;
        border-radius: var(--border-radius-item);
        border: 1px solid;

        &:disabled {
            cursor: not-allowed;
        }
    }

    &__radio {
        cursor: pointer;
    }

    &__select {
        padding: 0.5rem;
        border-radius: var(--border-radius-item);
        font-size: inherit;
    }

    &__label {
        cursor: pointer;
    }
}
</style>