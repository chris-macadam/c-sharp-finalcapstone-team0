import { createStore as _createStore } from 'vuex';
import axios from 'axios';

export function createStore(currentToken, currentUser) {
  let store = _createStore({
    state: {
      token: currentToken || '',
      user: currentUser || {},
      animals: [
        {
          id: 1,
          name: "Fluffy",
          age: 3,
          specialNeeds: false
          // Note: photoSrc is not set here; it will be handled by the getter
        },
        {
          id: 2,
          name: "Max",
          age: 2,
          specialNeeds: true
        },
        {
          id: 3,
          name: "Whiskers",
          age: 4,
          specialNeeds: false
        },
        {
          id: 4,
          name: "Rocky",
          age: 5,
          specialNeeds: true
        },
        {
          id: 5,
          name: "Sasha",
          age: 2,
          specialNeeds: false
        },
        {
          id: 6,
          name: "Sasha",
          age: 2,
          specialNeeds: false
        },
        {
          id: 7,
          name: "Sasha",
          age: 2,
          specialNeeds: false
        },
        {
          id: 8,
          name: "Sasha",
          age: 2,
          specialNeeds: false
        },
        {
          id: 9,
          name: "Sasha",
          age: 2,
          specialNeeds: false
        },
        {
          id: 10,
          name: "Sasha",
          age: 2,
          specialNeeds: false
        },
        
      ]
    },
    getters: {
      animalPhotos: state => {
        return state.animals.map((animal, index) => ({
          ...animal,
          photoSrc: `/PetPics - Copy/photo${index + 1}.jpg`
        }));
      }
    },
    mutations: {
      SET_AUTH_TOKEN(state, token) {
        state.token = token;
        localStorage.setItem('token', token);
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
      },
      SET_USER(state, user) {
        state.user = user;
        localStorage.setItem('user', JSON.stringify(user));
      },
      LOGOUT(state) {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        state.token = '';
        state.user = {};
        axios.defaults.headers.common = {};
      }
    },
  });
  return store;
}
